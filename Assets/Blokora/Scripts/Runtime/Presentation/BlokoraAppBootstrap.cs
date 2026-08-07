using Blokora.Domain;
using Blokora.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blokora.Presentation
{
    public sealed class BlokoraAppBootstrap : MonoBehaviour
    {
        private readonly Color background = new Color32(15, 23, 42, 255);
        private readonly Color surface = new Color32(30, 41, 59, 255);
        private readonly Color primary = new Color32(59, 130, 246, 255);
        private readonly Color accent = new Color32(251, 191, 36, 255);
        private BlokoraGameController controller;
        private RectTransform boardRoot;
        private RectTransform contentRoot;
        private RectTransform trayRoot;
        private Text scoreLabel;
        private Text statusLabel;
        private Text coinsLabel;
        private Text gemsLabel;
        private Text pageTitle;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateIfMissing()
        {
            if (FindObjectOfType<BlokoraAppBootstrap>() != null) return;
            var app = new GameObject("BlokoraApp");
            app.AddComponent<BlokoraAppBootstrap>();
        }

        private void Awake()
        {
            controller = gameObject.AddComponent<BlokoraGameController>(); controller.StartEndless();
            if (FindObjectOfType<EventSystem>() == null) { var events = new GameObject("EventSystem"); events.AddComponent<EventSystem>(); events.AddComponent<StandaloneInputModule>(); }
            BuildInterface();
        }

        private void BuildInterface()
        {
            var canvasObject = new GameObject("Canvas"); canvasObject.transform.SetParent(transform); var canvas = canvasObject.AddComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay; canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; canvasObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1080, 1920); canvasObject.AddComponent<GraphicRaycaster>();
            var root = Panel(canvasObject.transform, background, "Root"); Stretch(root);
            var top = Panel(root, new Color32(18, 30, 53, 255), "TopBar"); top.anchorMin = new Vector2(0, 1); top.anchorMax = new Vector2(1, 1); top.sizeDelta = new Vector2(0, 210); top.anchoredPosition = new Vector2(0, -105);
            Label(top, "BLOKORA", 52, accent, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -58), new Vector2(0, 76), TextAnchor.MiddleCenter);
            coinsLabel = Label(top, "COINS  500", 20, accent, new Vector2(0, 1), new Vector2(.5f, 1), new Vector2(90, -142), new Vector2(300, 44), TextAnchor.MiddleCenter);
            gemsLabel = Label(top, "GEMS  25", 20, new Color32(192, 132, 252, 255), new Vector2(.5f, 1), new Vector2(1, 1), new Vector2(-90, -142), new Vector2(300, 44), TextAnchor.MiddleCenter);
            contentRoot = Panel(root, new Color(0, 0, 0, 0), "Content"); contentRoot.anchorMin = Vector2.zero; contentRoot.anchorMax = Vector2.one; contentRoot.offsetMin = new Vector2(0, 225); contentRoot.offsetMax = new Vector2(0, -220);
            BuildSoloScreen();
            var nav = Panel(root, new Color32(18, 30, 53, 255), "Navigation"); nav.anchorMin = new Vector2(0, 0); nav.anchorMax = new Vector2(1, 0); nav.sizeDelta = new Vector2(0, 185); nav.anchoredPosition = new Vector2(0, 92);
            var navLayout = nav.gameObject.AddComponent<HorizontalLayoutGroup>(); navLayout.spacing = 10; navLayout.padding = new RectOffset(20, 20, 22, 22); navLayout.childAlignment = TextAnchor.MiddleCenter; navLayout.childForceExpandWidth = true;
            AddNavButton(nav, "HOME", () => BuildHomeScreen()); AddNavButton(nav, "SOLO", () => BuildSoloScreen()); AddNavButton(nav, "SHOP", () => BuildShopScreen()); AddNavButton(nav, "PROFILE", () => BuildProfileScreen());
            RefreshHeader();
        }

        private void BuildSoloScreen()
        {
            ClearContent();
            pageTitle = Label(contentRoot, "SOLO ENDLESS", 28, Color.white, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -45), new Vector2(0, 50), TextAnchor.MiddleCenter);
            scoreLabel = Label(contentRoot, "SCORE  0   •   COMBO 0", 24, Color.white, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -105), new Vector2(0, 50), TextAnchor.MiddleCenter);
            boardRoot = Panel(contentRoot, surface, "Board"); boardRoot.anchorMin = new Vector2(.5f, .5f); boardRoot.anchorMax = new Vector2(.5f, .5f); boardRoot.sizeDelta = new Vector2(700, 700); boardRoot.anchoredPosition = new Vector2(0, 120); var grid = boardRoot.gameObject.AddComponent<GridLayoutGroup>(); grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount; grid.constraintCount = 8; grid.cellSize = new Vector2(78, 78); grid.spacing = new Vector2(8, 8); grid.padding = new RectOffset(14, 14, 14, 14); DrawBoard(grid);
            statusLabel = Label(contentRoot, "DRAG A BLOCK ONTO THE BOARD", 18, new Color32(148, 163, 184, 255), new Vector2(0, .5f), new Vector2(1, .5f), new Vector2(0, -245), new Vector2(0, 45), TextAnchor.MiddleCenter);
            trayRoot = Panel(contentRoot, new Color32(20, 31, 52, 255), "Tray"); trayRoot.anchorMin = new Vector2(.5f, 0); trayRoot.anchorMax = new Vector2(.5f, 0); trayRoot.sizeDelta = new Vector2(900, 220); trayRoot.anchoredPosition = new Vector2(0, 190); var trayLayout = trayRoot.gameObject.AddComponent<HorizontalLayoutGroup>(); trayLayout.spacing = 18; trayLayout.childAlignment = TextAnchor.MiddleCenter; DrawTray(trayRoot);
            var restart = Button(contentRoot, controller.Session.IsGameOver ? "PLAY AGAIN" : "NEW RUN", primary); restart.anchorMin = new Vector2(.5f, 0); restart.anchorMax = new Vector2(.5f, 0); restart.sizeDelta = new Vector2(380, 68); restart.anchoredPosition = new Vector2(0, 80); restart.GetComponent<Button>().onClick.AddListener(() => { controller.Restart(); BuildSoloScreen(); });
            UpdateScore();
        }

        private void BuildHomeScreen() { ClearContent(); Label(contentRoot, "WELCOME BACK", 34, Color.white, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -120), new Vector2(0, 60), TextAnchor.MiddleCenter); Label(contentRoot, controller.Progress.UserName + "  •  LEVEL " + controller.Progress.Level, 22, new Color32(148, 163, 184, 255), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -180), new Vector2(0, 45), TextAnchor.MiddleCenter); AddBigAction("PLAY SOLO", primary, () => BuildSoloScreen(), -360); AddBigAction("RANKED BATTLE", new Color32(124, 58, 237, 255), () => ShowMessage("RANKED BATTLE", "Online competitive play will unlock when Blokora matchmaking is configured."), -470); AddBigAction("DAILY QUESTS", surface, () => ShowMessage("DAILY QUESTS", "Play a Solo run to progress your first quest: clear lines and build a combo."), -580); }
        private void BuildShopScreen() { ClearContent(); Label(contentRoot, "SHOP", 34, Color.white, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -100), new Vector2(0, 60), TextAnchor.MiddleCenter); Label(contentRoot, "COSMETICS NEVER CHANGE GAMEPLAY", 18, new Color32(148, 163, 184, 255), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -155), new Vector2(0, 40), TextAnchor.MiddleCenter); AddBigAction("CLASSIC SKIN   •   " + (controller.Progress.EquippedSkin == "classic" ? "EQUIPPED" : "EQUIP"), primary, () => { controller.Progress.TryPurchaseAndEquip("classic", 0); BuildShopScreen(); }, -300); AddBigAction("NEON SKIN   •   " + (controller.Progress.Owns("neon") ? "OWNED" : "250 COINS"), new Color32(30, 41, 59, 255), () => { controller.Progress.TryPurchaseAndEquip("neon", 250); BuildShopScreen(); }, -410); AddBigAction("DAILY REWARD   •   +100 COINS", new Color32(16, 185, 129, 255), () => { controller.Progress.TryClaimDailyReward(out _); BuildShopScreen(); }, -520); }
        private void BuildProfileScreen() { ClearContent(); Label(contentRoot, "PROFILE", 34, Color.white, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -95), new Vector2(0, 60), TextAnchor.MiddleCenter); Label(contentRoot, controller.Progress.UserName + "\nLEVEL " + controller.Progress.Level + "   XP " + controller.Progress.Xp + "\nHIGH SCORE  " + controller.Progress.HighScore + "\nGAMES  " + controller.Progress.GamesPlayed + "   LINES  " + controller.Progress.LinesCleared + "\nBEST COMBO  " + controller.Progress.BestCombo, 22, Color.white, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -300), new Vector2(800, 300), TextAnchor.MiddleCenter); AddBigAction("SETTINGS", surface, () => BuildSettingsScreen(), -570); }
        private void BuildSettingsScreen() { ClearContent(); Label(contentRoot, "SETTINGS", 34, Color.white, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -105), new Vector2(0, 60), TextAnchor.MiddleCenter); AddBigAction("MUSIC  •  " + (controller.Settings.Music ? "ON" : "OFF"), surface, () => { controller.Settings.SetMusic(!controller.Settings.Music); BuildSettingsScreen(); }, -260); AddBigAction("SOUND EFFECTS  •  " + (controller.Settings.SoundEffects ? "ON" : "OFF"), surface, () => { controller.Settings.SetSoundEffects(!controller.Settings.SoundEffects); BuildSettingsScreen(); }, -370); AddBigAction("HAPTICS  •  " + (controller.Settings.Haptics ? "ON" : "OFF"), surface, () => { controller.Settings.SetHaptics(!controller.Settings.Haptics); BuildSettingsScreen(); }, -480); AddBigAction("BACK TO PROFILE", primary, () => BuildProfileScreen(), -610); }
        private void AddBigAction(string text, Color color, UnityEngine.Events.UnityAction action, float y) { var button = Button(contentRoot, text, color); button.anchorMin = new Vector2(.5f, 1); button.anchorMax = new Vector2(.5f, 1); button.sizeDelta = new Vector2(620, 76); button.anchoredPosition = new Vector2(0, y); button.GetComponent<Button>().onClick.AddListener(action); }
        private void AddNavButton(Transform parent, string text, UnityEngine.Events.UnityAction action) { var button = Button(parent, text, new Color32(38, 55, 82, 255)); button.gameObject.GetComponent<LayoutElement>().preferredHeight = 100; button.GetComponent<Button>().onClick.AddListener(action); }
        private void ShowMessage(string title, string body) { ClearContent(); Label(contentRoot, title, 32, accent, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -200), new Vector2(0, 60), TextAnchor.MiddleCenter); Label(contentRoot, body, 21, Color.white, new Vector2(.12f, .5f), new Vector2(.88f, .5f), new Vector2(0, -40), new Vector2(0, 180), TextAnchor.MiddleCenter); AddBigAction("BACK HOME", primary, () => BuildHomeScreen(), -430); }
        private void ClearContent() { if (contentRoot == null) return; for (var i = contentRoot.childCount - 1; i >= 0; i--) Destroy(contentRoot.GetChild(i).gameObject); boardRoot = null; trayRoot = null; scoreLabel = null; statusLabel = null; }
        private void RefreshHeader() { if (coinsLabel != null) coinsLabel.text = "COINS  " + controller.Progress.Coins; if (gemsLabel != null) gemsLabel.text = "GEMS  " + controller.Progress.Gems; }

        private void DrawBoard(GridLayoutGroup grid)
        {
            for (var i = grid.transform.childCount - 1; i >= 0; i--) Destroy(grid.transform.GetChild(i).gameObject);
            for (var y = 0; y < controller.Session.Board.Height; y++) for (var x = 0; x < controller.Session.Board.Width; x++) { var color = controller.Session.Board.IsFilled(x, y) ? new Color32(59, 130, 246, 255) : new Color32(51, 65, 85, 255); var cell = Panel(grid.transform, color, $"Cell_{x}_{y}"); cell.gameObject.AddComponent<BoardCellView>().Initialize(controller, x, y); }
        }

        private void DrawTray(RectTransform tray)
        {
            for (var i = tray.childCount - 1; i >= 0; i--) Destroy(tray.GetChild(i).gameObject);
            for (var i = 0; i < controller.Pieces.Count; i++)
            {
                var piece = Panel(tray, new Color(0, 0, 0, 0), $"Piece_{i}");
                piece.sizeDelta = new Vector2(210, 190);
                DrawPiecePreview(piece, controller.Pieces[i]);
                piece.gameObject.AddComponent<PieceDragView>().Initialize(controller, i, boardRoot, this);
            }
        }

        private void DrawPiecePreview(RectTransform parent, PieceDefinition definition)
        {
            const float cell = 42f;
            const float gap = 6f;
            var minX = 99; var minY = 99; var maxX = -99; var maxY = -99;
            foreach (var point in definition.Cells) { minX = Mathf.Min(minX, point.x); minY = Mathf.Min(minY, point.y); maxX = Mathf.Max(maxX, point.x); maxY = Mathf.Max(maxY, point.y); }
            var centerX = (minX + maxX) * 0.5f; var centerY = (minY + maxY) * 0.5f;
            foreach (var point in definition.Cells)
            {
                var block = Panel(parent, primary, $"Block_{point.x}_{point.y}");
                block.sizeDelta = new Vector2(cell, cell);
                block.anchorMin = new Vector2(.5f, .5f); block.anchorMax = new Vector2(.5f, .5f);
                block.anchoredPosition = new Vector2((point.x - centerX) * (cell + gap), -(point.y - centerY) * (cell + gap));
                block.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

        public void Refresh() { if (boardRoot == null) return; DrawBoard(boardRoot.GetComponent<GridLayoutGroup>()); UpdateScore(); DrawTray(trayRoot); }
        private void UpdateScore() { if (scoreLabel != null) scoreLabel.text = $"SCORE  {controller.Session.Score}   •   COMBO {controller.Session.Combo}"; if (statusLabel != null) statusLabel.text = controller.Session.IsGameOver ? $"RUN COMPLETE  •  {controller.Session.Score} POINTS  •  {controller.Session.LinesCleared} LINES" : "DRAG A BLOCK ONTO THE BOARD"; RefreshHeader(); }
        public void ShowPlacementPreview(PieceDefinition piece, int x, int y)
        {
            if (boardRoot == null) return;
            var valid = controller.Session.Board.CanPlace(piece, x, y);
            var grid = boardRoot.GetComponent<GridLayoutGroup>();
            for (var cellIndex = 0; cellIndex < grid.transform.childCount; cellIndex++)
            {
                var cell = grid.transform.GetChild(cellIndex).GetComponent<Image>();
                var cellX = cellIndex % controller.Session.Board.Width; var cellY = cellIndex / controller.Session.Board.Width;
                var highlighted = false;
                foreach (var point in piece.Cells) highlighted |= cellX == x + point.x && cellY == y + point.y;
                if (highlighted) cell.color = valid ? new Color32(16, 185, 129, 255) : new Color32(239, 68, 68, 230);
            }
            if (statusLabel != null) statusLabel.text = valid ? "RELEASE TO PLACE" : "BLOCKED — FIND AN OPEN SPACE";
        }
        public void ClearPlacementPreview() { if (boardRoot == null) return; DrawBoard(boardRoot.GetComponent<GridLayoutGroup>()); UpdateScore(); }
        private static RectTransform Panel(Transform parent, Color color, string name) { var go = new GameObject(name); go.transform.SetParent(parent, false); var image = go.AddComponent<Image>(); image.color = color; return go.GetComponent<RectTransform>(); }
        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero; }
        private static Text Label(Transform parent, string text, int size, Color color, Vector2 min, Vector2 max, Vector2 position, Vector2 dimensions, TextAnchor anchor) { var go = new GameObject(text); go.transform.SetParent(parent, false); var rect = go.AddComponent<RectTransform>(); rect.anchorMin = min; rect.anchorMax = max; rect.sizeDelta = dimensions; rect.anchoredPosition = position; var label = go.AddComponent<Text>(); label.text = text; label.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); label.fontSize = size; label.color = color; label.alignment = anchor; label.fontStyle = FontStyle.Bold; return label; }
        private static RectTransform Button(Transform parent, string text, Color color) { var rect = Panel(parent, color, text); var button = rect.gameObject.AddComponent<Button>(); var layout = rect.gameObject.AddComponent<LayoutElement>(); layout.minHeight = 64; Label(rect, text, 24, Color.white, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, TextAnchor.MiddleCenter); return rect; }
    }

    public sealed class BoardCellView : MonoBehaviour
    {
        private BlokoraGameController controller; private int x; private int y;
        public void Initialize(BlokoraGameController game, int cellX, int cellY) { controller = game; x = cellX; y = cellY; }
    }

    public sealed class PieceDragView : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private BlokoraGameController controller; private int trayIndex; private RectTransform board; private BlokoraAppBootstrap app; private Vector2 originalPosition; private CanvasGroup group;
        public void Initialize(BlokoraGameController game, int index, RectTransform boardRoot, BlokoraAppBootstrap bootstrap) { controller = game; trayIndex = index; board = boardRoot; app = bootstrap; group = gameObject.AddComponent<CanvasGroup>(); }
        public void OnPointerDown(PointerEventData eventData) { originalPosition = ((RectTransform)transform).anchoredPosition; transform.SetAsLastSibling(); ((RectTransform)transform).localScale = Vector3.one * 1.12f; group.alpha = .88f; }
        public void OnDrag(PointerEventData eventData)
        {
            ((RectTransform)transform).position = eventData.position + new Vector2(0, 110);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(board, eventData.position, eventData.pressEventCamera, out var local);
            var x = Mathf.FloorToInt((local.x + board.rect.width / 2 - 14) / 86);
            var y = Mathf.FloorToInt((board.rect.height / 2 - local.y - 14) / 86);
            app.ShowPlacementPreview(controller.Pieces[trayIndex], x, y);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(board, eventData.position, eventData.pressEventCamera, out var local);
            var x = Mathf.FloorToInt((local.x + board.rect.width / 2 - 14) / 86);
            var y = Mathf.FloorToInt((board.rect.height / 2 - local.y - 14) / 86);
            var placed = controller.Place(trayIndex, x, y);
            if (!placed) { ((RectTransform)transform).anchoredPosition = originalPosition; if (controller.Settings.Haptics) Handheld.Vibrate(); }
            ((RectTransform)transform).localScale = Vector3.one; group.alpha = 1;
            if (placed) { if (controller.Settings.Haptics) Handheld.Vibrate(); app.Refresh(); }
            else app.ClearPlacementPreview();
        }
    }
}
