﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Overlays.Profile.Sections;
using osu.Game.Overlays.Profile.Sections.Ranks;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu.Mods;
using osu.Game.Rulesets.Scoring;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Desktop.VisualTests.Tests
{
    public class TestCaseUserRanks : TestCase
    {
        public override string Description => "showing your latest achievements";

        public TestCaseUserRanks()
        {
            RanksSection ranks;

            Add(new Container
            {
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = OsuColour.Gray(0.2f)
                    },
                    ranks = new RanksSection(),
                }
            });

            AddStep("Add First Place", () => ranks.FirstPlaceScores = new[]
            {
                new Score
                {
                    Rank = ScoreRank.A,
                    Accuracy = 0.735,
                    PP = 666,
                    Date = DateTimeOffset.UtcNow,
                    Mods = new Mod[] { new ModAutoplay(), new ModDoubleTime() },
                    Beatmap = new BeatmapInfo
                    {
                        Metadata = new BeatmapMetadata
                        {
                            Title = "FREEDOM DiVE",
                            Artist = "xi"
                        },
                        Version = "FOUR DIMENSIONS",
                        OnlineBeatmapID = 129891,
                    }
                }
            });

            AddStep("Add Best Performances", () =>
            {
                List<Score> scores = new List<Score>();
                Mod[] availableMods = { new OsuModHidden(), new OsuModFlashlight(), new OsuModHardRock(), new OsuModDoubleTime(), new OsuModPerfect() };
                List<Mod> selectedMods = new List<Mod>(availableMods);
                for (int i = 0; i <= availableMods.Length; i++)
                {
                    scores.Add(new Score
                    {
                        Rank = (ScoreRank) Enum.GetValues(typeof(ScoreRank)).GetValue(Enum.GetValues(typeof(ScoreRank)).Length - 1 - i),
                        Accuracy = Math.Pow(0.99, i),
                        PP = Math.Pow(0.5, i) * 800,
                        Date = DateTimeOffset.UtcNow.AddDays(-Math.Pow(i, 2)),
                        Mods = selectedMods.ToArray(),
                        Beatmap = new BeatmapInfo
                        {
                            Metadata = new BeatmapMetadata
                            {
                                Title = "Highscore",
                                Artist = "Panda Eyes & Teminite"
                            },
                            Version = "Game Over",
                            OnlineBeatmapID = 736215,
                        }
                    });
                    if(i < availableMods.Length)
                        selectedMods.Remove(availableMods[i]);
                }
                ranks.BestScores = scores;
            });
        }
    }
}
