﻿using System;
using System.Collections.Generic;
using System.Text;

using Landis.Core;
using Landis.Library.PnETCohorts;
using Landis.Library.AgeOnlyCohorts;
using Landis.SpatialModeling;

namespace Landis.Library.PnETCohorts
{

    public static class SiteVars
    {
        private static ISiteVar<float> siteRD;
        private static ISiteVar<Landis.Library.PnETCohorts.SiteCohorts> sitecohorts;

        public static void Initialize()
        {
            siteRD = EcoregionData.ModelCore.Landscape.NewSiteVar<float>();

            EcoregionData.ModelCore.RegisterSiteVar(siteRD, "Succession.SiteRd");
        }


        public static void SpeciesSiteRD(Landis.Library.PnETCohorts.SpeciesCohorts speciesCohorts, ActiveSite site)
        {
            float siteRD = 0;

            ISpeciesDensity speciesDensity = SpeciesParameters.SpeciesDensity.AllSpecies[speciesCohorts.Species.Index];
            //foreach (Landis.Library.PnETCohorts.ICohort cohort in speciesCohorts)
            for (int s = 0; s < speciesCohorts.Count; s++)
            {
                Landis.Library.PnETCohorts.ICohort cohort = speciesCohorts[s];
                double tmp_term1 = Math.Pow((cohort.Diameter / 25.4), 1.605);
                float tmp_term2 = 10000 / speciesDensity.MaxSDI;
                int tmp_term3 = cohort.Treenumber;
                double tmp = tmp_term1 * tmp_term2 * tmp_term3 / Math.Pow(EcoregionData.ModelCore.CellLength, 2);
                siteRD += (float)tmp;
            }

            SiteVars.SiteRD[site] = siteRD;
            //return siteRD;
        }

        public static void TotalSiteRD(Landis.Library.PnETCohorts.SiteCohorts cohorts)
        {

            float siteRD = 0;
            Landis.Library.PnETCohorts.Cohort.SetSiteAccessFunctions(cohorts);
            if (cohorts == null)
            {
                SiteVars.SiteRD[cohorts.Site] = siteRD;
            }
            else
            {
                foreach(Landis.Library.PnETCohorts.ICohort cohort in cohorts.AllCohorts)
                { 
                        double tmp_term1 = Math.Pow((cohort.Diameter / 25.4), 1.605);
                        float tmp_term2 = 10000 / SpeciesParameters.SpeciesDensity.AllSpecies[cohort.Species.Index].MaxSDI;
                        int tmp_term3 = cohort.Treenumber;
                        double tmp = tmp_term1 * tmp_term2 * tmp_term3 / Math.Pow(EcoregionData.ModelCore.CellLength, 2);
                        siteRD += (float)tmp;
                }

                SiteVars.SiteRD[cohorts.Site] = siteRD;
            }
            //return siteRD;
        }

        public static ISiteVar<float> SiteRD 
        {
            get
            {
                return siteRD;
            }
            
        }



    }
}
