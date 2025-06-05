using Core.MVP.Views;
using Game.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MVP.Presentation.Views
{
    public class MainUiView : CanvasGroupView
    {
        [field: SerializeField] public TMP_Text StonesCountText { get; private set; }
        [field: SerializeField] public GameObject StonesCounterObject { get; private set; }
        [field: SerializeField] public TMP_Text ProgressBarText { get; private set; }
        [field: SerializeField] public Image ProgressBarFillImage { get; private set; }
        [field: SerializeField] public GameObject ProgressBarObject { get; private set; }
        [field: SerializeField] public FillAnimation FillAnimation { get; private set; }
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public RectTransform StoneProgressPoint { get; private set; }
        [field: SerializeField] public RectTransform LeftMoneyPoint { get; private set; }
        [field: SerializeField] public RectTransform RightMoneyPoint { get; private set; }
        [field: SerializeField] public TMP_Text StoneProgressText { get; private set; }
        [field: SerializeField] public TMP_Text LeftMoneyText { get; private set; }
        [field: SerializeField] public TMP_Text RightMoneyText { get; private set; }
        [field: SerializeField] public TMP_Text MoneyCountText { get; private set; }
        [field: SerializeField] public TMP_Text HaveNotMoneyText { get; private set; }
    }
}
