﻿using Caliburn.Micro;
using IF.Lastfm.Core.Objects;
using System;

namespace Last.fm_Scrubbler_WPF.ViewModels
{
	/// <summary>
	/// ViewModel for the <see cref="Views.TrackResultView"/>.
	/// </summary>
	class FetchedTrackViewModel : PropertyChangedBase
	{
		#region Properties

		/// <summary>
		/// Event that triggers when <see cref="ToScrobble"/> changed.
		/// </summary>
		public event EventHandler ToScrobbleChanged;

		/// <summary>
		/// The fetched track.
		/// </summary>
		public LastTrack FetchedTrack
		{
			get { return _fetchedTrack; }
			private set
			{
				_fetchedTrack = value;
				NotifyOfPropertyChange(() => FetchedTrack);
			}
		}
		private LastTrack _fetchedTrack;

		/// <summary>
		/// Gets/sets if this scrobble should be scrobbled.
		/// </summary>
		public bool ToScrobble
		{
			get { return _toScrobble; }
			set
			{
				_toScrobble = value;
				NotifyOfPropertyChange(() => ToScrobble);
				ToScrobbleChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		private bool _toScrobble;

		#endregion Properties

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="fetchedTrack">The fetched track.</param>
		public FetchedTrackViewModel(LastTrack fetchedTrack)
		{
			FetchedTrack = fetchedTrack;
		}
	}
}