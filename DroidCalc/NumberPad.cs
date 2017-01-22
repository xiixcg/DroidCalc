using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DroidCalc
{
    [Activity(Label = "NumberPad")]
    public class NumberPad : Activity
    {
        private bool bIsDecimalRequired;
        private bool bIsFirstTimeEditingNumber = true;
        private bool bIsDecimalEntered = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "NumberPad" layout resource
            SetContentView(Resource.Layout.NumberPad);

            //Get our textview from the layout resource,
            //and attach an event to it
            TextView oNumber = FindViewById<TextView>(Resource.Id.NumberTextView);
            TextView oOne = FindViewById<TextView>(Resource.Id.OneTextView);
            TextView oTwo = FindViewById<TextView>(Resource.Id.TwoTextView);
            TextView oThree = FindViewById<TextView>(Resource.Id.ThreeTextView);
            TextView oFour = FindViewById<TextView>(Resource.Id.FourTextView);
            TextView oFive = FindViewById<TextView>(Resource.Id.FiveTextView);
            TextView oSix = FindViewById<TextView>(Resource.Id.SixTextView);
            TextView oSeven = FindViewById<TextView>(Resource.Id.SevenTextView);
            TextView oEight = FindViewById<TextView>(Resource.Id.EightTextView);
            TextView oNine = FindViewById<TextView>(Resource.Id.NineTextView);
            TextView oZero = FindViewById<TextView>(Resource.Id.ZeroTextView);
            TextView oDecimal = FindViewById<TextView>(Resource.Id.DecimalTextView);
            TextView oBackspace = FindViewById<TextView>(Resource.Id.BackspaceTextView);
            TextView oCancel = FindViewById<TextView>(Resource.Id.CancelTextView);
            TextView oOK = FindViewById<TextView>(Resource.Id.OKTextView);
                        
            //Get intents from sending activity
            bIsDecimalRequired = Intent.Extras.GetBoolean("IsDecimalRequired");
            oNumber.Text = Intent.Extras.GetString("Number");

            //OnClick response for each TextView
            oOne.Click += delegate
            {
                appendNumber(oNumber, "1");
            };
            oTwo.Click += delegate
            {
                appendNumber(oNumber, "2");
            };
            oThree.Click += delegate
            {
                appendNumber(oNumber, "3");
            };
            oFour.Click += delegate
            {
                appendNumber(oNumber, "4");
            };
            oFive.Click += delegate
            {
                appendNumber(oNumber, "5");
            };
            oSix.Click += delegate
            {
                appendNumber(oNumber, "6");
            };
            oSeven.Click += delegate
            {
                appendNumber(oNumber, "7");
            };
            oEight.Click += delegate
            {
                appendNumber(oNumber, "8");
            };
            oNine.Click += delegate
            {
                appendNumber(oNumber, "9");
            };
            oZero.Click += delegate
            {
                appendNumber(oNumber, "0");
            };
            oDecimal.Click += delegate
            {
                if (bIsDecimalRequired)
                {
                    bIsDecimalEntered = true;
                    appendNumber(oNumber, ".");
                }
            };
            oBackspace.Click += delegate
            {
                removeNumber(oNumber);
            };
            oCancel.Click += delegate
            {
                Intent oData = new Intent(this, typeof(MainActivity));
                SetResult(Result.Canceled, oData);
                Finish();
            };
            oOK.Click += delegate
            {
                if (isValidNumber(oNumber.Text))
                {
                    if (bIsDecimalRequired)
                    {
                        oNumber.Text = string.Format("{0:0.00}", Double.Parse(oNumber.Text));
                    }                    
                    Intent oData = new Intent(this, typeof(MainActivity));
                    oData.PutExtra("NewNumber", oNumber.Text);
                    SetResult(Result.Ok, oData);
                    Finish();
                }
            };
        }

        //Function to append a number or decimal
        private void appendNumber(TextView poTextView, string psNumber)
        {
            if (bIsFirstTimeEditingNumber)
            {
                if (psNumber == ".")
                {
                    psNumber = "0.";
                }
                poTextView.Text = psNumber;
                bIsFirstTimeEditingNumber = false;
            }
            else
            {
                poTextView.Text = poTextView.Text + psNumber;
                if (bIsDecimalRequired && bIsDecimalEntered)
                {
                    int nIndexOfDecimal = poTextView.Text.IndexOf(".");
                    if (poTextView.Text.Length - nIndexOfDecimal > 3)
                    {
                        poTextView.Text = poTextView.Text.Remove(poTextView.Text.Length - 1);
                    }                    
                }
            }                    
        }

        //Function to remove a number or decimal
        private void removeNumber(TextView poTextView)
        {
            if (poTextView.Text.Length != 0)
            {
                if (poTextView.Text.EndsWith("."))
                {
                    bIsDecimalEntered = false;
                }
                if (poTextView.Text.Length == 1)
                {
                    bIsFirstTimeEditingNumber = true;
                }
                poTextView.Text = poTextView.Text.Remove(poTextView.Text.Length - 1);
            }
        }

        //Validate if number is useable for calculation
        private bool isValidNumber(string psNumber)
        {
            if (psNumber.Length != 0 && psNumber != "0" && !psNumber.EndsWith("."))
            {
                return true;
            }
            return false;
        }
    }
}