using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Environment.Navigation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OrchardCore.Contents
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;

        public AdminMenu(
            IStringLocalizer<AdminMenu> localizer,
            IContentDefinitionManager contentDefinitionManager,
            IContentManager contentManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public void BuildNavigation(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            builder.Add(T["Content"], "1.4", content => content
                .AddClass("content").Id("content")
                .Add(T["Content Items"], "1", contentItems => contentItems
                    .Permission(Permissions.EditOwnContent)
                    .Action("List", "Admin", new { area = "OrchardCore.Contents" })
                    .LocalNav())
                );


            var contentTypeDefinitions = _contentDefinitionManager.ListTypeDefinitions().OrderBy(d => d.Name);
            var creatable = contentTypeDefinitions.Where(ctd => ctd.Settings.ToObject<ContentTypeSettings>().Creatable).OrderBy(ctd => ctd.DisplayName);
            if (creatable.Any())
            {
                builder.Add(T["New"], "-1", async newMenu =>
                {
                    newMenu.LinkToFirstChild(false).AddClass("new").Id("new");
                    foreach (var contentTypeDefinition in creatable)
                    {
                        var ci = await _contentManager.NewAsync(contentTypeDefinition.Name);
                        var cim = await _contentManager.PopulateAspectAsync<ContentItemMetadata>(ci);
                        var createRouteValues = cim.CreateRouteValues;
                        if (createRouteValues.Any())
                            newMenu.Add(new LocalizedString(contentTypeDefinition.DisplayName, contentTypeDefinition.DisplayName), "5", item => item
                                .Action(cim.CreateRouteValues["Action"] as string, cim.CreateRouteValues["Controller"] as string, cim.CreateRouteValues)
                                // Apply "PublishOwn" permission for the content type
                                //.Permission(DynamicPermissions.CreateDynamicPermission(DynamicPermissions.PermissionTemplates[Permissions.PublishOwnContent.Name], contentTypeDefinition)
                                );
                    }
                });
            }
        }
    }
}