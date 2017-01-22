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
    public class Calculate : Activity
    {
        public Activity oActivity;
        public Calculate(Activity poActivity)
        {
            this.oActivity = poActivity;
        }
        //Calculate Total Per Person
        public string calculateTotalPerPerson(TextView poTotal, TextView poNumberOfPeople)
        {
            double nTotal = Double.Parse(poTotal.Text);
            double nNumberOfPeople = Double.Parse(poNumberOfPeople.Text);
            double nTotalPerPerson = nTotal / nNumberOfPeople;
            return nTotalPerPerson.ToString("F");
        }
        //Calculate Total with Tip according to Tip Percent, set the Tip Amount while returning Total with Tip
        public string calculateTotalWithTip(string psTotalPerPerson, string psTipPercent, TextView poTip)
        {
            double nTotalPerPerson = Double.Parse(psTotalPerPerson);
            double nTipPercent = Double.Parse(psTipPercent) / 100;
            double nTip = nTotalPerPerson * nTipPercent;
            poTip.Text = nTip.ToString("F");
            double nTotalWithTip = nTotalPerPerson + nTip;
            return nTotalWithTip.ToString("F");
        }
        //Calculate Total with Tip accordint to Tip amount, set the Tip Percent while returning Total with Tip
        public string calculateTotalWithTip(string psTotalPerPerson, TextView poTipPercent, string psTip)
        {
            double nTotalPerPerson = Double.Parse(psTotalPerPerson);
            double nTip = Double.Parse(psTip);
            double nTipPercent = nTip / nTotalPerPerson * 100;
            poTipPercent.Text = nTipPercent.ToString("F");
            double nTotalWithTip = nTotalPerPerson + nTip;
            return nTotalWithTip.ToString("F");
        }
        //Calculate Tip Percent and Tip amount according to Total with Tip
        public void calculateTipAndPercent(string psTotalPerPerson, TextView poTipPercent, TextView poTip, string psTotalWithTip)
        {
            double nTotalPerPerson = Double.Parse(psTotalPerPerson);
            double nTotalWithTip = Double.Parse(psTotalWithTip);
            double nTip = nTotalWithTip - nTotalPerPerson;
            poTip.Text = nTip.ToString("F");
            double nTipPercent = nTip / nTotalPerPerson * 100;
            poTipPercent.Text = nTipPercent.ToString("F");
        }
    }
}