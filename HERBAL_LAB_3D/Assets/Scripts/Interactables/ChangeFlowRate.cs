using UnityEngine;
using UnityEngine.UI;

public class FlowControl : MonoBehaviour
{
    public Button flowRateUp;
    public Button flowRateDown;

    public void selectFlowUp()
    {
        ColorBlock upColors = flowRateUp.colors;
        flowRateUp.GetComponent<Image>().color = upColors.selectedColor;

        ColorBlock downColors = flowRateDown.colors;
        flowRateDown.GetComponent<Image>().color = downColors.disabledColor;
    }

    public void selectFlowDown()
    {
        ColorBlock downColors = flowRateDown.colors;
        flowRateDown.GetComponent<Image>().color = downColors.selectedColor;

        ColorBlock upColors = flowRateUp.colors;
        flowRateUp.GetComponent<Image>().color = upColors.disabledColor;
    }
}
