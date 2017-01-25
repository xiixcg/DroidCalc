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
using System.IO;
using Android.Graphics;
using Android.Provider;
using Android.Content.PM;

namespace DroidCalc
{
    [Activity(Label = "DroidCalc", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        TextView oTextView;
        string sDirectoryPath;
        bool bIsPictureTaken = true;

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

            //Check if there is app to take picture
            if (IsThereAnAppToTakePictures())
            {
                //Create directory for the picture to store
                CreateDirectoryForPictures();
                //OnClick response for Capture button
                oCaptureReceipt.Click += TakeAPicture;          
            }

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
                if (oTextView != null)
                {
                    oTextView.Text = oData.Extras.GetString("NewNumber");
                }
            }

            // Make it available in the gallery
            if (!bIsPictureTaken)
            {
                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);

                // Dispose of the Java side bitmap.
                GC.Collect();
                bIsPictureTaken = true;
            }
        }

        private void CreateDirectoryForPictures()
        {
            //Sucessfully created directory but can't use it to set the picture as content yet
            //sDirectoryPath = "/sdcard/Calculate/";
            //Directory.CreateDirectory(sDirectoryPath);   

            App._dir = new Java.IO.File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "ReceiptHistory");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
         }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            //Sucessfully created directory and file but can't use it to set the returning pictur as content yet
            //sDirectoryPath = sDirectoryPath + String.Format("my_receipt{0}.jpg", (DateTime.Now).ToFileTime());
            //System.IO.File.Create(sDirectoryPath);
            //System.Console.WriteLine("DateForm: " + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond);
            //intent.PutExtra(MediaStore.ExtraOutput, sDirectoryPath);

            bIsPictureTaken = false;
            oTextView = null;
            Intent intent = new Intent(MediaStore.ActionImageCapture);            
            App._file = new Java.IO.File(App._dir, String.Format("myReceipt{0}{1}{2}_{3}.jpg", DateTime.Now.Year, 
                DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));
            StartActivityForResult(intent, 0);
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
    public static class App
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;
        public static Bitmap bitmap;
    }
}


