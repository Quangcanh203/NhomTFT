using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DoAn.Repository.Components
{
	public class CategoryViewComponent : ViewComponent
	{
		private readonly DataContext _dataContext;
		public CategoryViewComponent(DataContext context)
		{
			_dataContext = context;

		}
		public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.category.ToListAsync());
	}
}
