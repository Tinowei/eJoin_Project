using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Web.Models;
using Web.ViewModels.HostViewModel;

namespace Web.Services.HostService
{
    public class HostService
    {
        private readonly IEventRepository _eventRepoForPage;

        public HostService(IEventRepository eventRepoForPage)
        {
            _eventRepoForPage = eventRepoForPage;
        }

        public async Task<ThemeSelectViewModel> GetThemeSelectViewModel()
        {
            try
            {
                var themeData = await _eventRepoForPage.GetAllThemes().ToListAsync();

                ThemeSelectViewModel themeSelectViewModel = new ThemeSelectViewModel()
                {
                    Theme = themeData.Select(t => new Themes
                    {
                        Id = t.Id,
                        Image = t.IconUrl,
                        Name = t.ThemeName
                    }).ToList(),
                };

                return themeSelectViewModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}