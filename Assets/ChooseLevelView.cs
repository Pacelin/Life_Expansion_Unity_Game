using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelView : MonoBehaviour
{
    [SerializeField] private Button _levelOne;
    [SerializeField] private Button _levelTwo;
    [SerializeField] private Button _levelThree;
    [SerializeField] private Button _levelFour;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        _levelOne.onClick.AddListener(PressButtonOne);
        _levelTwo.onClick.AddListener(PressButtonTwo);
        _levelThree.onClick.AddListener(PressButtonThree);
        _levelFour.onClick.AddListener(PressButtonFour);
    }

    private void OnDisable()
    {
        _levelOne.onClick.RemoveListener(PressButtonOne);
        _levelTwo.onClick.RemoveListener(PressButtonTwo);
        _levelThree.onClick.RemoveListener(PressButtonThree);
        _levelFour.onClick.RemoveListener(PressButtonFour);
    }

    public void PressButtonOne()
    {
        
    }
    public void PressButtonTwo()
    {

    }
    public void PressButtonThree()
    {

    }
    public void PressButtonFour()
    {

    }
}
