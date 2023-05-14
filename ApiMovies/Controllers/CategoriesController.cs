using ApiMovies.Entities;
using ApiMovies.Entities.DTOs;
using ApiMovies.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AllowAnonymousAttribute = Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute;

namespace ApiMovies.Controllers
{
    /// <summary>
    /// Categories Controller
    /// ControllerBase: Indica que es un controlador para API.
    /// </summary>
    // [Authorize]
    // [Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [AllowAnonymous] // Este es opcional si quiero que el end point este abierto a todos
        [HttpGet]
        // [ResponseCache(Duration = 20)] // 20 segundos de cache
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)] // No queremos guardar el cache con errores, y que los errores se muestren constantemente
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories().ToList();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

            return Ok(categoriesDto);
        }

        [AllowAnonymous]
        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        // [ResponseCache(Duration = 30)] // 30 segundos de cache
        [ResponseCache(CacheProfileName = "PorDefecto20Segundos")] // Cache Profile
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory(int categoryId)
        {
            var category = _categoryRepository.GetCategory(categoryId);

            if (category is null)
            {
                return NotFound();
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            // ModelState: En el modelo habiamos indicado algunos campos obligatorios, se validane esas condiciones
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createCategoryDto is null)
            {
                return BadRequest(ModelState);
            }

            if(_categoryRepository.CategoryExists(createCategoryDto.Name))
            {
                ModelState.AddModelError("", $"The category {createCategoryDto.Name} already exists.");
                return StatusCode(404, ModelState);
            }

            var category = _mapper.Map<Category>(createCategoryDto);

            if (!_categoryRepository.CreateCategory(category))
            {
                ModelState.AddModelError("", $"Something went wrong while saving the category {category.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = category.Id }, category);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{categoryId:int}", Name = "UpdatePatchCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePatchCategory(int categoryId, [FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoryDto is null || categoryId != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);

            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the category {category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var category = _categoryRepository.GetCategory(categoryId);

            if (!_categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the category {category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
