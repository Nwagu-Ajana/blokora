using Blokora.Domain;
using Blokora.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
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
        private Text scoreLabel;
        private Text statusLabel;

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
            if (FindObjectOfType<EventSystem>() == null) { var events = new GameObject("EventSystem"); events.AddComponent<EventSystem>(); events.AddComponent<InputSystemUIInputModule>(); }
            BuildInterface();
        }

        private void BuildInterface()
        {
            var canvasObject = new GameObject("Canvas"); canvasObject.transform.SetParent(transform); var canvas = canvasObject.AddComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay; canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; canvasObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1080, 1920); canvasObject.AddComponent<GraphicRaycaster>();
            var root = Panel(canvasObject.transform, background, "Root"); Stretch(root);
            Label(root, "BLOKORA", 56, accent, new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -100), new Vector2(900, 90), TextAnchor.MiddleCenter);
            Label(root, "ENDLESS RUN", 22, new Color32(148, 163, 184, 255), new Vector2(.5f, 1), new Vector2(.5f, 1), new Vector2(0, -165), new Vector2(900, 50), TextAnchor.MiddleCenter);
            scoreLabel = Label(root, "SCORE  0", 30, Color.white, new Vector2(.5f, 1), new Vector2(.5f, 1), new Vector2(0, -235), new Vector2(900, 60), TextAnchor.MiddleCenter);
            boardRoot = Panel(root, surface, "Board"); boardRoot.anchorMin = new Vector2(.5f, .5f); boardRoot.anchorMax = new Vector2(.5f, .5f); boardRoot.sizeDelta = new Vector2(760, 760); boardRoot.anchoredPosition = new Vector2(0, 100); var grid = boardRoot.gameObject.AddComponent<GridLayoutGroup>(); grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount; grid.constraintCount = 8; grid.cellSize = new Vector2(86, 86); grid.spacing = new Vector2(9, 9); grid.padding = new RectOffset(16, 16, 16, 16); DrawBoard(grid);
            statusLabel = Label(root, "DRAG A BLOCK ONTO THE BOARD", 20, new Color32(148, 163, 184, 255), new Vector2(.5f, .5f), new Vector2(.5f, .5f), new Vector2(0, -320), new Vector2(900, 50), TextAnchor.MiddleCenter);
            var tray = Panel(root, new Color32(20, 31, 52, 255), "Tray"); tray.anchorMin = new Vector2(.5f, 0); tray.anchorMax = new Vector2(.5f, 0); tray.sizeDelta = new Vector2(900, 260); tray.anchoredPosition = new Vector2(0, 210); var trayLayout = tray.gameObject.AddComponent<HorizontalLayoutGroup>(); trayLayout.spacing = 34; trayLayout.childAlignment = TextAnchor.MiddleCenter; DrawTray(tray);
            var restart = Button(root, "NEW RUN", primary); restart.anchorMin = new Vector2(.5f, 0); restart.anchorMax = new Vector2(.5f, 0); restart.sizeDelta = new Vector2(420, 86); restart.anchoredPosition = new Vector2(0, 90); restart.GetComponent<Button>().onClick.AddListener(() => { controller.Restart(); DrawBoard(boardRoot.GetComponent<GridLayoutGroup>()); DrawTray(tray); UpdateScore(); });
            Label(root, "SOLO FOUNDATION • RANKED COMING SOON", 16, new Color32(100, 116, 139, 255), new Vector2(.5f, 0), new Vector2(.5f, 0), new Vector2(0, 35), new Vector2(900, 40), TextAnchor.MiddleCenter);
        }

        private void DrawBoard(GridLayoutGroup grid)
        {
            for (var i = grid.transform.childCount - 1; i >= 0; i--) Destroy(grid.transform.GetChild(i).gameObject);
            for (var y = 0; y < controller.Session.Board.Height; y++) for (var x = 0; x < controller.Session.Board.Width; x++) { var color = controller.Session.Board.IsFilled(x, y) ? new Color32(59, 130, 246, 255) : new Color32(51, 65, 85, 255); var cell = Panel(grid.transform, color, $"Cell_{x}_{y}"); cell.gameObject.AddComponent<BoardCellView>().Initialize(controller, x, y); }
        }

        private void DrawTray(RectTransform tray)
        {
            for (var i = tray.childCount - 1; i >= 0; i--) Destroy(tray.GetChild(i).gameObject);
            for (var i = 0; i < controller.Pieces.Count; i++) { var piece = Panel(tray, new Color32(59, 130, 246, 255), $"Piece_{i}"); piece.sizeDelta = new Vector2(210, 190); piece.gameObject.AddComponent<PieceDragView>().Initialize(controller, i, boardRoot, this); }
        }

        public void Refresh() { DrawBoard(boardRoot.GetComponent<GridLayoutGroup>()); UpdateScore(); }
        private void UpdateScore() { if (scoreLabel != null) scoreLabel.text = $"SCORE  {controller.Session.Score}   •   COMBO {controller.Session.Combo}"; if (statusLabel != null) statusLabel.text = controller.Session.IsGameOver ? $"RUN COMPLETE  •  {controller.Session.PiecesPlaced} PIECES PLACED" : "DRAG A BLOCK ONTO THE BOARD"; }
        private static RectTransform Panel(Transform parent, Color color, string name) { var go = new GameObject(name); go.transform.SetParent(parent, false); var image = go.AddComponent<Image>(); image.color = color; return go.AddComponent<RectTransform>(); }
        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero; }
        private static Text Label(Transform parent, string text, int size, Color color, Vector2 min, Vector2 max, Vector2 position, Vector2 dimensions, TextAnchor anchor) { var go = new GameObject(text); go.transform.SetParent(parent, false); var rect = go.AddComponent<RectTransform>(); rect.anchorMin = min; rect.anchorMax = max; rect.sizeDelta = dimensions; rect.anchoredPosition = position; var label = go.AddComponent<Text>(); label.text = text; label.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); label.fontSize = size; label.color = color; label.alignment = anchor; label.fontStyle = FontStyle.Bold; return label; }
        private static RectTransform Button(Transform parent, string text, Color color) { var rect = Panel(parent, color, text); var button = rect.gameObject.AddComponent<Button>(); Label(rect, text, 24, Color.white, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, TextAnchor.MiddleCenter); return rect; }
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
        public void OnPointerDown(PointerEventData eventData) { originalPosition = ((RectTransform)transform).anchoredPosition; ((RectTransform)transform).localScale = Vector3.one * 1.12f; group.alpha = .88f; }
        public void OnDrag(PointerEventData eventData) { ((RectTransform)transform).position = eventData.position + new Vector2(0, 100); }
        public void OnPointerUp(PointerEventData eventData) { var local = board.InverseTransformPoint(eventData.position); var x = Mathf.FloorToInt((local.x + board.rect.width / 2 - 16) / 95); var y = Mathf.FloorToInt((board.rect.height / 2 - local.y - 16) / 95); var placed = controller.Place(trayIndex, x, y); if (!placed) ((RectTransform)transform).anchoredPosition = originalPosition; ((RectTransform)transform).localScale = Vector3.one; group.alpha = 1; if (placed) app.Refresh(); }
    }
}
