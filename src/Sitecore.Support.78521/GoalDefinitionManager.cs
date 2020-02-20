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
using Sitecore.Marketing.Definitions.Goals;
using Sitecore.Marketing.Definitions.Goals.Data;
using Sitecore.Marketing.ObservableFeed.Activation;
using Sitecore.Marketing.ObservableFeed.DeleteDefinition;
using Sitecore.Marketing.Search;
using Sitecore.Xdb.Configuration;

namespace Sitecore.Support.Marketing.Definitions.Goals
{
    public class GoalDefinitionManager : Sitecore.Marketing.Definitions.Goals.GoalDefinitionManager
    {
        public GoalDefinitionManager(IGoalDefinitionRepository repository, ITaxonomyClassificationResolver<IGoalDefinition> classificationResolver, IDefinitionFieldLabelResolver definitionFieldLabelResolver, IDefinitionSearchProvider<IGoalDefinition> searchProvider, IActivationObservableFeed<IGoalDefinition> activationFeed, IDeleteDefinitionObservableFeed<IGoalDefinition> deleteDefinitionFeed, IDefinitionManagerSettings settings) : base(repository, classificationResolver, definitionFieldLabelResolver, searchProvider, activationFeed, deleteDefinitionFeed, settings)
        {
        }

        public override IGoalDefinition Get(Guid id, CultureInfo cultureInfo, bool includeInactiveVersion)
        {
            Condition.Requires(id, "id").IsNotEmptyGuid();
            Condition.Requires(cultureInfo, "cultureInfo").IsNotNull();
            GoalDefinitionRecord val = Repository.Get(id, cultureInfo, includeInactiveVersion);
            if (val != null)
            {
                return ConvertRecordToDefinition(val, cultureInfo);
            }
            foreach (Language language in LanguageManager.GetLanguages(Factory.GetDatabase(XdbSettings.DefaultDefinitionDatabase)))
            {
                GoalDefinitionRecord val2 = Repository.Get(id, language.CultureInfo, includeInactiveVersion);
                if (val2 != null)
                {
                    var goal = ConvertRecordToDefinition(val2, language.CultureInfo);
                    goal.Name = goal.Alias;
                    return goal;
                }
            }
            return default;
        }
    }
}