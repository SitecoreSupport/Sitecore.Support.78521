using Sitecore.Abstractions;
using Sitecore.Analytics.Configuration;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Marketing.Core.ObservableFeed;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.Goals;
using Sitecore.Marketing.Definitions.Goals.Data;
using Sitecore.Marketing.Search;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Sitecore.Support.Marketing.Definitions.Goals
{
    public class GoalDefinitionManager : Sitecore.Marketing.Definitions.Goals.GoalDefinitionManager
    {
        // Fields
        private readonly IGoalDefinitionRepository repository;

        // Methods
        [Obsolete("Use the overload that takes an IDefinitionSearchProvider")]
        public GoalDefinitionManager(IGoalDefinitionRepository repository, ITaxonomyClassificationResolver classificationResolver) : this(repository, classificationResolver, false)
        {
            Assert.ArgumentNotNull(repository, "repository");
            Assert.ArgumentNotNull(classificationResolver, "classificationResolver");
            this.repository = repository;
        }

        [Obsolete("Use the overload that takes an IDefinitionSearchProvider")]
        public GoalDefinitionManager(IGoalDefinitionRepository repository, ITaxonomyClassificationResolver classificationResolver, bool isReadOnly) : this(repository, classificationResolver, new NotSupportedSearchProvider<IGoalDefinition>(), isReadOnly)
        {
            Assert.ArgumentNotNull(repository, "repository");
            Assert.ArgumentNotNull(classificationResolver, "classificationResolver");
            this.repository = repository;
        }

        public GoalDefinitionManager(IGoalDefinitionRepository repository, ITaxonomyClassificationResolver classificationResolver, IDefinitionSearchProvider<IGoalDefinition> searchProvider, bool isReadOnly = false) : base(repository, classificationResolver, searchProvider, isReadOnly)
        {
            Assert.ArgumentNotNull(repository, "repository");
            Assert.ArgumentNotNull(classificationResolver, "classificationResolver");
            Assert.ArgumentNotNull(searchProvider, "searchProvider");
            this.repository = repository;
        }

        public GoalDefinitionManager(IGoalDefinitionRepository repository, ITaxonomyClassificationResolver classificationResolver, IDefinitionFieldLabelResolver definitionFieldLabelResolver, IDefinitionSearchProvider<IGoalDefinition> searchProvider, bool isReadOnly = false) : base(repository, classificationResolver, searchProvider, isReadOnly)
        {
            Assert.ArgumentNotNull(repository, "repository");
            Assert.ArgumentNotNull(classificationResolver, "classificationResolver");
            Assert.ArgumentNotNull(definitionFieldLabelResolver, "definitionFieldLabelResolver");
            Assert.ArgumentNotNull(searchProvider, "searchProvider");
            base.FieldLabelResolver = definitionFieldLabelResolver;
            this.repository = repository;
        }

        public GoalDefinitionManager(IGoalDefinitionRepository repository, ITaxonomyClassificationResolver classificationResolver, Sitecore.Marketing.Search.IDefinitionSearchProvider<IGoalDefinition> searchProvider, ICorePipeline pipelines, bool isReadOnly = false) : base(repository, classificationResolver, searchProvider, pipelines, isReadOnly)
        {
            Assert.ArgumentNotNull(repository, "repository");
            Assert.ArgumentNotNull(classificationResolver, "classificationResolver");
            Assert.ArgumentNotNull(searchProvider, "searchProvider");
            Assert.ArgumentNotNull(pipelines, "pipelines");
            this.repository = repository;
        }

        public GoalDefinitionManager(IGoalDefinitionRepository repository, ITaxonomyClassificationResolver classificationResolver, Sitecore.Marketing.Search.IDefinitionSearchProvider<IGoalDefinition> searchProvider, ICorePipeline pipelines, bool isReadOnly, IObservableFeed<IGoalDefinition> activationFeed) : base(repository, classificationResolver, searchProvider, pipelines, isReadOnly, activationFeed)
        {
            Assert.ArgumentNotNull(repository, "repository");
            Assert.ArgumentNotNull(classificationResolver, "classificationResolver");
            Assert.ArgumentNotNull(searchProvider, "searchProvider");
            Assert.ArgumentNotNull(pipelines, "pipelines");
            Assert.ArgumentNotNull(activationFeed, "activationFeed");
            this.repository = repository;
        }

        public override IGoalDefinition Get(ID id, CultureInfo cultureInfo, bool includeInactiveVersion)
        {
            Assert.ArgumentNotNull(id, "id");
            Assert.ArgumentNotNull(cultureInfo, "cultureInfo");
            GoalDefinitionRecord record = this.repository.Get(id, cultureInfo, includeInactiveVersion);
            if (record != null)
            {
                return this.ToGoalDefinition(record, cultureInfo);
            }
            record = this.repository.Get(id, Context.Language.CultureInfo, includeInactiveVersion);
            if (record != null)
            {
                return this.ToGoalDefinition(record, Context.Language.CultureInfo);
            }
            foreach (Language language in LanguageManager.GetLanguages(Configuration.Factory.GetDatabase(AnalyticsSettings.DefaultDefinitionDatabase)))
            {
                record = this.repository.Get(id, language.CultureInfo, includeInactiveVersion);
                if (record != null)
                {
                    return this.ToGoalDefinition(record, language.CultureInfo);
                }
            }
            return null;
        }
    }
}