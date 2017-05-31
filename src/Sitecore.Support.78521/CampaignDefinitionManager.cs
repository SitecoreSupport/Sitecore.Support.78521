using Sitecore.Marketing.Definitions.Campaigns;
using Sitecore.Diagnostics;
using Sitecore.Marketing.Definitions.Campaigns.Data;
using Sitecore.Marketing.Search;
using Sitecore.Marketing.Definitions;
using Sitecore.Abstractions;
using Sitecore.Marketing.Core.ObservableFeed;
using System;
using Sitecore.Data;
using System.Globalization;
using Sitecore.Globalization;
using Sitecore.Data.Managers;
using Sitecore.Analytics.Configuration;

namespace Sitecore.Support.Marketing.Definitions.Campaigns
{
    public class CampaignDefinitionManager : Sitecore.Marketing.Definitions.Campaigns.CampaignDefinitionManager
    {
        private readonly ICampaignDefinitionRepository repository;

        public CampaignDefinitionManager(ICampaignDefinitionRepository repository,
            ITaxonomyClassificationResolver classificationResolver,
            IDefinitionFieldLabelResolver definitionFieldLabelResolver,
            IDefinitionSearchProvider<ICampaignActivityDefinition> searchProvider, bool isReadOnly)
            : base(repository, classificationResolver, definitionFieldLabelResolver, searchProvider, isReadOnly)
        {
            Assert.ArgumentNotNull(repository, "repository");
            Assert.ArgumentNotNull(classificationResolver, "classificationResolver");
            Assert.ArgumentNotNull(definitionFieldLabelResolver, "definitionFieldLabelResolver");
            Assert.ArgumentNotNull(searchProvider, "searchProvider");
            this.repository = repository;
        }

        public CampaignDefinitionManager(ICampaignDefinitionRepository repository,
            ITaxonomyClassificationResolver classificationResolver,
            IDefinitionSearchProvider<ICampaignActivityDefinition> searchProvider, ICorePipeline pipelines,
            bool isReadOnly)
            : base(repository, classificationResolver, searchProvider, pipelines, isReadOnly)
        {
            Assert.ArgumentNotNull(repository, "repository");
            Assert.ArgumentNotNull(classificationResolver, "classificationResolver");
            Assert.ArgumentNotNull(searchProvider, "searchProvider");
            Assert.ArgumentNotNull(pipelines, "pipelines");
            this.repository = repository;
        }

        public CampaignDefinitionManager(ICampaignDefinitionRepository repository,
            ITaxonomyClassificationResolver classificationResolver,
            IDefinitionSearchProvider<ICampaignActivityDefinition> searchProvider, ICorePipeline pipelines,
            bool isReadOnly, IObservableFeed<ICampaignActivityDefinition> activationFeed)
            : base(repository, classificationResolver, searchProvider, pipelines, isReadOnly, activationFeed)
        {
            Assert.ArgumentNotNull(repository, "repository");
            Assert.ArgumentNotNull(classificationResolver, "classificationResolver");
            Assert.ArgumentNotNull(searchProvider, "searchProvider");
            Assert.ArgumentNotNull(pipelines, "pipelines");
            Assert.ArgumentNotNull(activationFeed, "activationFeed");
            this.repository = repository;
        }

        public CampaignDefinitionManager(ICampaignDefinitionRepository repository,
            ITaxonomyClassificationResolver classificationResolver,
            IDefinitionSearchProvider<ICampaignActivityDefinition> searchProvider, bool isReadOnly = false)
            : base(repository, classificationResolver, searchProvider, isReadOnly)
        {
            Assert.ArgumentNotNull(repository, "repository");
            Assert.ArgumentNotNull(classificationResolver, "classificationResolver");
            Assert.ArgumentNotNull(searchProvider, "searchProvider");
            this.repository = repository;
        }

        [Obsolete("Use the overloads that take an IDefinitionSearchProvider")]
        public CampaignDefinitionManager(ICampaignDefinitionRepository repository, ITaxonomyClassificationResolver classificationResolver, bool isReadOnly)
            : this(repository, classificationResolver, new NotSupportedSearchProvider<ICampaignActivityDefinition>(), new CorePipelineWrapper(), isReadOnly)
        {
            Assert.ArgumentNotNull(repository, "repository");
            Assert.ArgumentNotNull(classificationResolver, "classificationResolver");
            this.repository = repository;
        }

        [Obsolete("Use the overloads that take an IDefinitionSearchProvider")]
        public CampaignDefinitionManager(ICampaignDefinitionRepository repository, ITaxonomyClassificationResolver classificationResolver)
            : this(repository, classificationResolver, new NotSupportedSearchProvider<ICampaignActivityDefinition>(), new CorePipelineWrapper(), false)
        {
            Assert.ArgumentNotNull(repository, "repository");
            this.repository = repository;
        }

        public override ICampaignActivityDefinition Get(ID id, CultureInfo cultureInfo, bool includeInactiveVersion)
        {
            Assert.ArgumentNotNull(id, "id");
            Assert.ArgumentNotNull(cultureInfo, "cultureInfo");
            CampaignActivityDefinitionRecord campaignActivityDefinitionRecord = this.repository.Get(id, cultureInfo, includeInactiveVersion);
            if (campaignActivityDefinitionRecord != null)
            {
                return this.ToCampaignActivityDefinition(campaignActivityDefinitionRecord, cultureInfo);
            }
            campaignActivityDefinitionRecord = this.repository.Get(id, Context.Language.CultureInfo, includeInactiveVersion);
            if (campaignActivityDefinitionRecord != null)
            {
                return this.ToCampaignActivityDefinition(campaignActivityDefinitionRecord, Context.Language.CultureInfo);
            }
            foreach (Language language in LanguageManager.GetLanguages(Configuration.Factory.GetDatabase(AnalyticsSettings.DefaultDefinitionDatabase)))
            {
                campaignActivityDefinitionRecord = this.repository.Get(id, language.CultureInfo, includeInactiveVersion);
                if (campaignActivityDefinitionRecord != null)
                {
                    return this.ToCampaignActivityDefinition(campaignActivityDefinitionRecord, language.CultureInfo);
                }
            }
            return null;
        }
    }
}