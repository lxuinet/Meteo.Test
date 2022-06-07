using System;
using System.Collections.Generic;
using System.Text;
using LX;

namespace Meteo.Controls
{
    public class ScrollableLabel : Label
    {
        protected override void DoDown(MouseEventArgs e)
        {
            base.DoDown(e);
            CanSelection = false;
            Trimming = false;
            StopTimer("scroll");
        }

        protected override void DoUp(MouseEventArgs e)
        {
            base.DoUp(e);
            DoScroll();
        }

        protected override void DoScroll()
        {
            base.DoScroll();

            StopTimer("scroll");
            StartTimer("scroll", 1000).Tick += delegate
            {
                Trimming = true;
                StopTimer("scroll");
            };
        }
        
    }
}
