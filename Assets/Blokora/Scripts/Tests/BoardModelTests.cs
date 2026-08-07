using System.Collections.Generic;
using Blokora.Domain;
using Blokora.Gameplay;
using NUnit.Framework;

namespace Blokora.Tests
{
    public sealed class BoardModelTests
    {
        [Test] public void InvalidPlacementDoesNotMutateBoard()
        {
            var board = new BoardModel(4, 4); var piece = new PieceDefinition("line", (0, 0), (1, 0));
            Assert.That(board.CanPlace(piece, 3, 0), Is.False); Assert.That(board.IsFilled(0, 0), Is.False);
        }

        [Test] public void CompletedRowClears()
        {
            var board = new BoardModel(4, 4); var single = new PieceDefinition("single", (0, 0));
            for (var x = 0; x < 4; x++) Assert.That(board.Place(single, x, 0).Lines, Is.EqualTo(x == 3 ? 1 : 0));
            for (var x = 0; x < 4; x++) Assert.That(board.IsFilled(x, 0), Is.False);
        }

        [Test] public void FullColumnClears()
        {
            var board = new BoardModel(4, 4); var single = new PieceDefinition("single", (0, 0));
            for (var y = 0; y < 4; y++) board.Place(single, 0, y);
            Assert.That(board.IsFilled(0, 0), Is.False);
        }

        [Test] public void GameOverOnlyWhenNoPieceFits()
        {
            var board = new BoardModel(2, 2); var single = new PieceDefinition("single", (0, 0));
            Assert.That(board.HasAnyValidPlacement(new List<PieceDefinition> { single }), Is.True);
            board.Place(single, 0, 0); board.Place(single, 1, 0); board.Place(single, 0, 1); board.Place(single, 1, 1);
            Assert.That(board.HasAnyValidPlacement(new List<PieceDefinition> { single }), Is.False);
        }

        [Test] public void SameSeedProducesSameSequence()
        {
            var a = new PieceGenerator(42); var b = new PieceGenerator(42);
            for (var i = 0; i < 20; i++) Assert.That(a.Next().Id, Is.EqualTo(b.Next().Id));
        }

        [Test] public void DifferentSeedsCanVary()
        {
            var a = new PieceGenerator(1); var b = new PieceGenerator(2); var different = false;
            for (var i = 0; i < 20; i++) different |= a.Next().Id != b.Next().Id;
            Assert.That(different, Is.True);
        }

        [Test] public void ScoreRulesAreCentralized()
        {
            Assert.That(ScoreRules.Placement(4), Is.EqualTo(20)); Assert.That(ScoreRules.Lines(2, 3), Is.EqualTo(375));
        }

        [Test] public void CatalogContainsVerticalAndCompoundPatterns()
        {
            var vertical = false; var compound = false;
            foreach (var piece in PieceCatalog.All) { vertical |= piece.Id == "line4v"; compound |= piece.Id == "plus"; }
            Assert.That(vertical, Is.True); Assert.That(compound, Is.True);
        }

        [Test] public void ClearResultReportsRowsAndColumnsSeparately()
        {
            var result = new ClearResult(2, 1, 10);
            Assert.That(result.Lines, Is.EqualTo(3));
            Assert.That(result.Rows, Is.EqualTo(2));
            Assert.That(result.Columns, Is.EqualTo(1));
        }

        [Test] public void SoloSessionKeepsExactlyThreePieces()
        {
            var session = new SoloGameSession(42);
            Assert.That(session.Tray, Has.Count.EqualTo(3));
            Assert.That(session.TryPlace(0, 0, 0, out _), Is.True);
            Assert.That(session.Tray, Has.Count.EqualTo(3));
        }

        [Test] public void ClearLabelsAreReadable()
        {
            Assert.That(ScoreRules.ClearLabel(1, 0), Is.EqualTo("SINGLE CLEAR"));
            Assert.That(ScoreRules.ClearLabel(2, 0), Is.EqualTo("DOUBLE CLEAR"));
            Assert.That(ScoreRules.ClearLabel(1, 1), Is.EqualTo("CROSS CLEAR"));
        }
    }
}
