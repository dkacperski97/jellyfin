using System;
using System.IO;
using Jellyfin.Extensions;
using MediaBrowser.Controller.Lyrics;
using MediaBrowser.Controller.Resolvers;

namespace MediaBrowser.Providers.Lyric;

/// <summary>
/// TXT Lyric Parser.
/// </summary>
public class TxtLyricParser : ILyricParser
{
    private static readonly string[] _supportedMediaTypes = { "lrc", "elrc", "txt" };
    private static readonly string[] _lineBreakCharacters = { "\r\n", "\r", "\n" };

    /// <inheritdoc />
    public string Name => "TxtLyricProvider";

    /// <summary>
    /// Gets the priority.
    /// </summary>
    /// <value>The priority.</value>
    public ResolverPriority Priority => ResolverPriority.Fifth;

    /// <inheritdoc />
    public LyricResponse? ParseLyrics(LyricFile lyrics)
    {
        if (!_supportedMediaTypes.Contains(Path.GetExtension(lyrics.Name.AsSpan())[1..], StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        string[] lyricTextLines = lyrics.Content.Split(_lineBreakCharacters, StringSplitOptions.None);

        if (lyricTextLines.Length == 0)
        {
            return null;
        }

        LyricLine[] lyricList = new LyricLine[lyricTextLines.Length];

        for (int lyricLineIndex = 0; lyricLineIndex < lyricTextLines.Length; lyricLineIndex++)
        {
            lyricList[lyricLineIndex] = new LyricLine(lyricTextLines[lyricLineIndex]);
        }

        return new LyricResponse { Lyrics = lyricList };
    }
}
