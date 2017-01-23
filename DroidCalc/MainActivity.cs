using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Text;
using System.Globalization;
using Android.Content;
using Android.Views;
using System.Collections.Generic;
using Android.Runtime;
using Java.IO;
using Android.Graphics;

namespace DroidCalc
{
    [Activity(Label = "DroidCalc", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        TextView oTextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            //Get our textview from the layout resource,
            //and attach an event to it
            TextView oTotal = FindViewById<TextView>(Resource.Id.TotalTextView);
            TextView oPeopleCountMinus = FindViewById<TextView>(Resource.Id.PeopleCountMinusTextView);
            TextView oPeopleCount = FindViewById<TextView>(Resource.Id.PeopleCountTextView);
            TextView oPeopleCountPlus = FindViewById<TextView>(Resource.Id.PeopleCountPlusTextView);
            TextView oTotalPerPerson = FindViewById<TextView>(Resource.Id.TotalPerPersonTextView);
            TextView oTipPercentMinus = FindViewById<TextView>(Resource.Id.TipPercentMinusTextView);
            TextView oTipPercent = FindViewById<TextView>(Resource.Id.TipPercentTextView);
            TextView oTipPercentPlus = FindViewById<TextView>(Resource.Id.TipPercentPlusTextView);
            TextView oTipMinus = FindViewById<TextView>(Resource.Id.TipMinusTextView);
            TextView oTip = FindViewById<TextView>(Resource.Id.TipTextView);
            TextView oTipPlus = FindViewById<TextView>(Resource.Id.TipPlusTextView);
            TextView oTotalWithTipMinus = FindViewById<TextView>(Resource.Id.TotalWithTipMinusTextView);
            TextView oTotalWithTip = FindViewById<TextView>(Resource.Id.TotalWithTipTextView);
            TextView oTotalWithTipPlus = FindViewById<TextView>(Resource.Id.TotalWithTipPlusTextView);

            //Get our button from the layout resource,
            //and attach an event to it
            Button oCaptureReceipt = FindViewById<Button>(Resource.Id.captureReceiptButton);

            //Create EditNumber object with current view
            EditNumber oEditor = new EditNumber(this);
            //Create Calculate object with current view
            Calculate oCalculate = new Calculate(this);

            //OnClick response for each TextView object
            oTotal.Click += delegate
            {
                Intent oIntent = oEditor.intentForNumberEditor(oTotal);
                oTextView = oTotal;
                StartActivityForResult(oIntent, 0);
            };
            oPeopleCountMinus.Click += delegate
            {
                oPeopleCount.Text = oEditor.editNumberDecrement(oPeopleCount.Text);
            };
            oPeopleCount.Click += delegate
            {
                Intent oIntent = oEditor.intentForNumberEditor(oPeopleCount);
                oTextView = oPeopleCount;
                StartActivityForResult(oIntent, 0);
            };
            oPeopleCountPlus.Click += delegate
            {
                oPeopleCount.Text = oEditor.editNumberIncrement(oPeopleCount.Text);
            };
            oTipPercentMinus.Click += delegate
            {
                oTextView = oTipPercent;
                oTipPercent.Text = oEditor.editNumberDecrement(oTipPercent.Text);
            };
            oTipPercent.Click += delegate
            {
                Intent oIntent = oEditor.intentForNumberEditor(oTipPercent);
                oTextView = oTipPercent;
                StartActivityForResult(oIntent, 0);
            };
            oTipPercentPlus.Click += delegate
            {
                oTextView = oTipPercent;
                oTipPercent.Text = oEditor.editNumberIncrement(oTipPercent.Text);
            };
            oTipMinus.Click += delegate
            {
                oTextView = oTip;
                oTip.Text = oEditor.editNumberDecrement(oTip.Text);
            };
            oTip.Click += delegate
            {
                Intent oIntent = oEditor.intentForNumberEditor(oTip);
                oTextView = oTip;
                StartActivityForResult(oIntent, 0);
            };
            oTipPlus.Click += delegate
            {
                oTextView = oTip;
                oTip.Text = oEditor.editNumberIncrement(oTip.Text);
            };
            oTotalWithTipMinus.Click += delegate
            {
                oTextView = oTotalWithTip;
                oTotalWithTip.Text = oEditor.editNumberDecrement(oTotalWithTip.Text);
            };
            oTotalWithTip.Click += delegate
            {
                Intent oIntent = oEditor.intentForNumberEditor(oTotalWithTip);
                oTextView = oTotalWithTip;
                StartActivityForResult(oIntent, 0);
            };
            oTotalWithTipPlus.Click += delegate
            {
                oTextView = oTotalWithTip;
                oTotalWithTip.Text = oEditor.editNumberIncrement(oTotalWithTip.Text);
            };

            //Onclick response for button
            oCaptureReceipt.Click += delegate
            {
            
            };

            //OnTextChange response for each TextView
            oTotal.AfterTextChanged += delegate
            {
                oTotalPerPerson.Text =  oCalculate.calculateTotalPerPerson(oTotal, oPeopleCount);
                oTotalWithTip.Text = oCalculate.calculateTotalWithTip(oTotalPerPerson.Text, oTipPercent.Text, oTip);
            };
            oPeopleCount.AfterTextChanged += delegate
            {
                oTotalPerPerson.Text = oCalculate.calculateTotalPerPerson(oTotal, oPeopleCount);
                oTotalWithTip.Text = oCalculate.calculateTotalWithTip(oTotalPerPerson.Text, oTipPercent.Text, oTip);
            };
            oTipPercent.AfterTextChanged += delegate
            {
                if (oTextView == oTipPercent)
                {
                    oTotalWithTip.Text = oCalculate.calculateTotalWithTip(oTotalPerPerson.Text, oTipPercent.Text, oTip);
                }
            };
            oTip.AfterTextChanged += delegate
            {
                if (oTextView == oTip)
                {
                    oTotalWithTip.Text = oCalculate.calculateTotalWithTip(oTotalPerPerson.Text, oTipPercent, oTip.Text);
                }
            };
            oTotalWithTip.AfterTextChanged += delegate
            {
                if (oTextView == oTotalWithTip)
                {
                    oCalculate.calculateTipAndPercent(oTotalPerPerson.Text, oTipPercent, oTip, oTotalWithTip.Text);
                }                
            };
        }

        protected override void OnActivityResult(int nRequestCode, [GeneratedEnum] Result eResultCode, Intent oData)
        {
            base.OnActivityResult(nRequestCode, eResultCode, oData);
            if (eResultCode == Result.Ok)
            {
                oTextView.Text = oData.Extras.GetString("NewNumber");
            }
        }
    }
        
    public class EditNumber : Activity
    {
        public Activity oActivity;
        public EditNumber(Activity poActivity)
        {
            this.oActivity = poActivity;
        }
        //Decrease number by 1
        public string editNumberDecrement(string psNumber)
        {
            int nIndexOfDecimal = psNumber.IndexOf(".");
            double nNewNumber = Double.Parse(psNumber);
            if (nNewNumber >= 1)
            {
                nNewNumber -= 1;
            }
            else
            {
                nNewNumber = 0;
            }
            if (nIndexOfDecimal >= 0)
            {
                return nNewNumber.ToString("F2");
            }
            return nNewNumber.ToString();
        }
        //Increase number by 1
        public string editNumberIncrement(string psNumber)
        {
            int nIndexOfDecimal = psNumber.IndexOf(".");
            double nNewNumber = Double.Parse(psNumber);
            nNewNumber += 1;
            if (nIndexOfDecimal >= 0)
            {
                return nNewNumber.ToString("F2");
            }
            return nNewNumber.ToString();
        }
        //Open number pad to change number
        public Intent intentForNumberEditor(TextView poTextView)
        {
            int nIndexOfDecimal = poTextView.Text.IndexOf(".");
            Intent oIntent = new Intent(oActivity, typeof(NumberPad));
            oIntent.PutExtra("IsDecimalRequired", nIndexOfDecimal >= 0 ? true : false);
            oIntent.PutExtra("Number", poTextView.Text);
            return oIntent;
        }
    }
}


