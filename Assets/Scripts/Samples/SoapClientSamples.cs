using System;
using UnityEngine;

namespace VRTX.Net.Samples
{
    public class SoapClientSamples : MonoBehaviour
    {
        [SerializeField()]
        private TMPro.TextMeshProUGUI _results = null;
        [SerializeField()]
        private TMPro.TMP_InputField _input = null;

        // service url:
        // http://www.thomas-bayer.com/axis2/services/BLZService?wsdl
        // service base url:
        // http://www.thomas-bayer.com/axis2/services/BLZService
        // service namespace:
        // http://thomas-bayer.com/blz/

        private BLZService.BLZServiceSoapClient _blzServiceClient = new BLZService.BLZServiceSoapClient();
        private Calculator.CalculatorSoapClient _calculatorClient = new Calculator.CalculatorSoapClient();
        // Start is called before the first frame update
        public async void ExecuteBLZRequest()
        {
            BLZService.GetBankRequest request = new BLZService.GetBankRequest()
            { BLZ = _input.text };

            // concise way of sending request using the SoapOperation implementation
            BLZService.GetBankResponse response = await BLZService.GetBank.Execute(_blzServiceClient, request);
            if (response != null)
            {
                _results.text = "Result:" + Environment.NewLine;
                _results.text += response.Bezeichnung.ToString() + Environment.NewLine;
                _results.text += response.BIC.ToString() + Environment.NewLine;
                _results.text += response.PLZ.ToString() + " " + response.Ort.ToString();
            }
        }

        public async void ExecuteCalculatorAddRequest()
        {
            Calculator.AddRequest request = new Calculator.AddRequest()
            {
                intA = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue),
                intB = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue)
            };
            Calculator.AddResponse response = await Calculator.Add.Execute(_calculatorClient, request);
            if (response != null)
                _results.text = string.Format("{0} + {1} = {2}", request.intA, request.intB, response.Value) + Environment.NewLine;
        }
        public async void ExecuteCalculatorSubtractRequest()
        {
            Calculator.SubtractRequest request = new Calculator.SubtractRequest()
            {
                intA = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue),
                intB = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue)
            };
            Calculator.SubtractResponse response = await Calculator.Subtract.Execute(_calculatorClient, request);
            if (response != null)
                _results.text = string.Format("{0} - {1} = {2}", request.intA, request.intB, response.Value) + Environment.NewLine;
        }
        public async void ExecuteCalculatorMultiplyRequest()
        {
            Calculator.MultiplyRequest request = new Calculator.MultiplyRequest()
            {
                intA = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue),
                intB = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue)
            };
            Calculator.MultiplyResponse response = await Calculator.Multiply.Execute(_calculatorClient, request);
            if (response != null)
                _results.text = string.Format("{0} * {1} = {2}", request.intA, request.intB, response.Value) + Environment.NewLine;
        }
        public async void ExecuteCalculatorDivideRequest()
        {
            Calculator.DivideRequest request = new Calculator.DivideRequest()
            {
                intA = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue),
                intB = UnityEngine.Random.Range(1, byte.MaxValue / 16)
            };
            Calculator.DivideResponse response = await Calculator.Divide.Execute(_calculatorClient, request);
            if (response != null)
                _results.text = string.Format("{0} / {1} = {2}", request.intA, request.intB, response.Value) + Environment.NewLine;
        }

    }

}