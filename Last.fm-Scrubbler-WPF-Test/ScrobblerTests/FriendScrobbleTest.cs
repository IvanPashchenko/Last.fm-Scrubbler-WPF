﻿using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Scrobblers;
using Scrubbler.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scrubbler.ViewModels.ScrobbleViewModels;

namespace Scrubbler.Test.ScrobblerTests
{
  /// <summary>
  /// Tests for the <see cref="FriendScrobbleViewModel"/>.
  /// </summary>
  [TestFixture]
  class FriendScrobbleTest
  {
    /// <summary>
    /// Tests the <see cref="FriendScrobbleViewModel.Scrobble"/> function.
    /// </summary>
    /// <returns>Task.</returns>
    [Test]
    public async Task ScrobbleTest()
    {
      //given: FriendScrobbleViewModel with mocked userapi
      List<Scrobble> expected = new List<Scrobble>
      {
        new Scrobble("TestArtist", "TestAlbum", "TestTrack", DateTime.Now) { Duration = TimeSpan.FromSeconds(30) },
        new Scrobble("TestArtist2", "TestAlbum2", "TestTrack2", DateTime.Now.AddSeconds(1)) { Duration = TimeSpan.FromSeconds(30) },
        new Scrobble("TestArtist3", "TestAlbum3", "TestTrack3", DateTime.Now.AddSeconds(2)) { Duration = TimeSpan.FromSeconds(30) }
      };

      Mock<IAuthScrobbler> scrobblerMock = new Mock<IAuthScrobbler>();
      IEnumerable<Scrobble> actual = null;
      scrobblerMock.Setup(i => i.ScrobbleAsync(It.IsAny<IEnumerable<Scrobble>>())).Callback<IEnumerable<Scrobble>>(s => actual = s)
                                                                                  .Returns(Task.Run(() => new ScrobbleResponse()));


      Mock<IUserApi> userApiMock = new Mock<IUserApi>();
      userApiMock.Setup(i => i.GetRecentScrobbles(It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>(), It.IsAny<int>())).Returns(()
                                                  => Task.Run(() => PageResponse<LastTrack>.CreateSuccessResponse(expected.ToLastTracks())));

      FriendScrobbleViewModel vm = new FriendScrobbleViewModel(null, userApiMock.Object)
      {
        Scrobbler = scrobblerMock.Object,
        Username = "TestUser"
      };

      await vm.FetchScrobbles();
      vm.SelectAll();

      // when: scrobbling the fetched tracks.
      await vm.Scrobble();

      // we add 1 second to each TimePlayed of the expected because the vm does this too so you can scrobble from yourself...
      for(int i = 0; i < expected.Count; i++)
        expected[i] = expected[i].CloneWithAddedSecond();

      // then: scrobbled tracks should be equal to the given tracks.
      Assert.That(actual.IsEqualScrobble(expected), Is.True);
    }
  }
}