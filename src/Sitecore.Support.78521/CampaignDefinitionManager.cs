using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Sitecore.Configuration;
using Sitecore.Data.Managers;
using Sitecore.Framework.Conditions;
using Sitecore.Globalization;
using Sitecore.Marketing.Core.Extensions;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.Campaigns;
using Sitecore.Marketing.Definitions.Campaigns.Data;
using Sitecore.Marketing.ObservableFeed.Activation;
using Sitecore.Marketing.ObservableFeed.DeleteDefinition;
using Sitecore.Marketing.Search;
using Sitecore.Xdb.Configuration;

namespace Sitecore.Support.Marketing.Definitions.Campaigns
{
    public class CampaignDefinitionManager : Sitecore.Marketing.Definitions.Campaigns.CampaignDefinitionManager
    {

        public CampaignDefinitionManager(ICampaignDefinitionRepository repository, ITaxonomyClassificationResolver<ICampaignActivityDefinition> classificationResolver, IDefinitionFieldLabelResolver definitionFieldLabelResolver, IDefinitionSearchProvider<ICampaignActivityDefinition> searchProvider, IActivationObservableFeed<ICampaignActivityDefinition> activationFeed, IDeleteDefinitionObservableFeed<ICampaignActivityDefinition> deleteDefinitionFeed, IDefinitionManagerSettings settings) : base(repository, classificationResolver, definitionFieldLabelResolver, searchProvider, activationFeed, deleteDefinitionFeed, settings)
        {
        }
        public override ICampaignActivityDefinition Get(Guid id, CultureInfo cultureInfo, bool includeInactiveVersion)
        {
            Condition.Requires(id, "id").IsNotEmptyGuid();
            Condition.Requires(cultureInfo, "cultureInfo").IsNotNull();
            CampaignActivityDefinitionRecord val = Repository.Get(id, cultureInfo, includeInactiveVersion);
            if (val != null)
            {
                return ConvertRecordToDefinition(val, cultureInfo);
            }
            foreach (Language language in LanguageManager.GetLanguages(Factory.GetDatabase(XdbSettings.DefaultDefinitionDatabase)))
            {
                CampaignActivityDefinitionRecord val2 = Repository.Get(id, language.CultureInfo, includeInactiveVersion);
                if (val2 != null)
                {
                    var campaign = ConvertRecordToDefinition(val2, language.CultureInfo);
                    campaign.Name = campaign.Alias;
                    return campaign;
                }
            }
            return default;
        }
    }
}