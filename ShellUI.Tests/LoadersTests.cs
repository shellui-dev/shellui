using System.Linq;
using ShellUI.CLI.Services;
using Xunit;

namespace ShellUI.Tests;

public class LoadersTests
{
    [Fact]
    public void SnakeSpinner_WalksTheGridEdgeWithAThreeDotTrail()
    {
        var frames = SnakeSpinner.Instance.Frames;

        Assert.Equal(8, frames.Count);
        Assert.Equal(8, frames.Distinct().Count());
        foreach (var frame in frames)
        {
            Assert.Equal(2, frame.Length);
            Assert.All(frame, ch => Assert.InRange(ch, '⠀', '⣿'));
            Assert.Equal(3, frame.Sum(ch => System.Numerics.BitOperations.PopCount((uint)(ch - 0x2800))));
        }

        // The centre cell (row 1, column 1 → first character, dot 0x10) is never part of the snake path.
        Assert.DoesNotContain(frames, f => ((f[0] - 0x2800) & 0x10) != 0);
    }

    [Fact]
    public void Braille_MapsGridCellsToDots()
    {
        Assert.Equal("⠉⠁", SnakeSpinner.Braille(i => i < 3));
        Assert.Equal("⠇⠀", SnakeSpinner.Braille(i => i % 3 == 0));
    }

    [Fact]
    public void LogoLoader_BuildsTheMarkAlongTheDiagonalThenClearsIt()
    {
        Assert.Equal(new[] { "X....", ".....", ".....", ".....", "....." }, LogoLoader.Frame(0));
        Assert.Equal(new[] { "X...X", ".X..X", "..X.X", ".X..X", "X...X" }, LogoLoader.Frame(8));
        Assert.Equal(LogoLoader.Frame(8), LogoLoader.Frame(14));
        Assert.Equal(new[] { ".....", ".....", ".....", ".....", "....." }, LogoLoader.Frame(29));
        Assert.Equal(LogoLoader.Frame(0), LogoLoader.Frame(30));
    }
}
