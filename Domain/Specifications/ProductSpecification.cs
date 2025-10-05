using Domain.Entities;

namespace Domain.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(Guid? categoryId = null, bool? isActive = null, string? searchTerm = null)
        {
            AddInclude(p => p.Category);

            if (categoryId.HasValue)
            {
                SetCriteria(p => p.CategoryId == categoryId.Value);
            }

            if (isActive.HasValue)
            {
                var currentCriteria = Criteria;
                if (currentCriteria != null)
                {
                    SetCriteria(p => currentCriteria.Compile()(p) && p.IsActive == isActive.Value);
                }
                else
                {
                    SetCriteria(p => p.IsActive == isActive.Value);
                }
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var currentCriteria = Criteria;
                if (currentCriteria != null)
                {
                    SetCriteria(p => currentCriteria.Compile()(p) &&
                        (p.Name.Value.Contains(searchTerm) || p.Description.Contains(searchTerm)));
                }
                else
                {
                    SetCriteria(p => p.Name.Value.Contains(searchTerm) || p.Description.Contains(searchTerm));
                }
            }

            AddOrderBy(p => p.Name.Value);
        }
    }
}
