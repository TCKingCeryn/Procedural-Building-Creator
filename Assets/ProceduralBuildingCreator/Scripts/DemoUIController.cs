 using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;
using ModularBuildingsFramework;


namespace ModularBuildingsFramework.Demo
{
	public class DemoUIController : MonoBehaviour
	{

        public DemoGenerator demo;
        [SerializeField] private Dropdown buildersDropdown = default(Dropdown);

        [Space]
        [SerializeField] private Text lengthLabel = default(Text);
        [SerializeField] private Slider lengthSlider = default(Slider);

        [Space]
        [SerializeField] private Text heightLabel = default(Text);
        [SerializeField] private Slider heightSlider = default(Slider);

        [Space]
        [SerializeField] private Text depthLabel = default(Text);
        [SerializeField] private Slider depthSlider = default(Slider);

        [Space]
        [SerializeField] private Toggle decoratorsToggle = default(Toggle);
        [SerializeField] private Button generateButton = default(Button);



        void Awake()
        {

            if(!demo) demo = FindObjectOfType<DemoGenerator>();

            buildersDropdown.ClearOptions();

            foreach (var builder in demo.GetBuilders())
            {
                var option = new Dropdown.OptionData(builder.name);
                buildersDropdown.options.Add(option);
            }

            buildersDropdown.value = 1;
            buildersDropdown.value = 0;

            lengthSlider.value = 20f;
            heightSlider.value = 20f;
            depthSlider.value = 20f;
            
            decoratorsToggle.isOn = true;

            RedrawLengthText();
            RedrawHeightText();
            RedrawDepthText();
        }
        void OnEnable()
        {
            buildersDropdown.onValueChanged.AddListener(OnBuildersDropdownValueChanged);

            lengthSlider.onValueChanged.AddListener(OnLengthSliderValueChanged);
            heightSlider.onValueChanged.AddListener(OnHeightSliderValueChanged);
            depthSlider.onValueChanged.AddListener(OnDepthSliderValueChanged);

            decoratorsToggle.onValueChanged.AddListener(OnDecoratorsToggleChanged);
            generateButton.onClick.AddListener(OnGenerateButtonClicked);
        }
        void OnDisable()
        {
            buildersDropdown.onValueChanged.RemoveListener(OnBuildersDropdownValueChanged);

            lengthSlider.onValueChanged.RemoveListener(OnLengthSliderValueChanged);
            heightSlider.onValueChanged.RemoveListener(OnHeightSliderValueChanged);
            depthSlider.onValueChanged.RemoveListener(OnDepthSliderValueChanged);

            decoratorsToggle.onValueChanged.RemoveListener(OnDecoratorsToggleChanged);
            generateButton.onClick.RemoveListener(OnGenerateButtonClicked);
        }




        public void OnBuildersDropdownValueChanged(int value)
        {
            demo.builderIndex = value;
        }



        public void OnLengthSliderValueChanged(float value)
        {
            demo.draftLength = value;

            RedrawLengthText();
        }
        public void OnHeightSliderValueChanged(float value)
        {
            demo.draftHeight = value;

            RedrawHeightText();
        }
        public void OnDepthSliderValueChanged(float value)
        {
            demo.draftDepth = value;

            RedrawDepthText();
        }


        public void OnDecoratorsToggleChanged(bool value)
        {
            demo.isRunDecorators = value;
        }
        public void OnGenerateButtonClicked()
        {
            demo.RequestRebuild();
        }



        void RedrawLengthText()
        {
            lengthLabel.text = "Length: " + Mathf.RoundToInt(lengthSlider.value);
        }
        void RedrawHeightText()
        {
            heightLabel.text = "Height: " + Mathf.RoundToInt(heightSlider.value);
        }
        void RedrawDepthText()
        {
            depthLabel.text = "Depth: " + Mathf.RoundToInt(depthSlider.value);
        }

       
       
    }



}