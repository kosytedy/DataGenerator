using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataGeneratorSample.Data;
using DataGeneratorSample.Models;
using DataGeneratorSample.DTOs;
using System.Dynamic;
using Newtonsoft.Json;

namespace DataGeneratorSample.Controllers
{
    public class ModelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Model
        public async Task<IActionResult> Index()
        {
            return View(await _context.Models.ToListAsync());
        }

        // GET: Model/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .Include(x => x.Models)
                .Include(x => x.Properties)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            var baseModel = new ExpandoObject() as IDictionary<string, object>;

            var levels = new Dictionary<string, int>();

            BuildData(model, ref baseModel, ref levels, 0);

            string modelResponseSample = JsonConvert.SerializeObject(baseModel, Formatting.Indented);

            ViewData["modelResponseSample"] = $"\n{modelResponseSample}";

            return View(model);
        }

        // GET: Model/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Models"] = await _context.Models.ToListAsync();
            ViewData["Properties"] = await _context.Property.ToListAsync();
            return View();
        }

        // POST: Model/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewModel newModel)
        {
            if (ModelState.IsValid)
            {
                Model model = new Model();
                model.Id = Guid.NewGuid();
                model.Name = newModel.Name;
                model.Endpoint = newModel.Endpoint;

                if(newModel.Models != null && newModel.Models.Count > 0)
                {
                    List<Model> models = await _context.Models.Where(x => newModel.Models.Contains(x.Id)).ToListAsync();
                    model.Models = models;
                }

                if (newModel.Properties != null && newModel.Properties.Count > 0)
                {
                    List<Property> properties = await _context.Property.Where(x => newModel.Properties.Contains(x.Id)).ToListAsync();
                    model.Properties = properties;
                }

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newModel);
        }

        // GET: Model/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Model/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Endpoint")] Model model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Model/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Model/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var model = await _context.Models.Include(x => x.Models).Include(x => x.Properties).FirstOrDefaultAsync(x => x.Id == id);
            model.Models = null;
            model.Properties = null;
            _context.Update(model);
            _context.Models.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModelExists(Guid id)
        {
            return _context.Models.Any(e => e.Id == id);
        }

        public IDictionary<string, object> BuildData(Model model, ref IDictionary<string, object> baseModel, ref Dictionary<string, int> levels, int currentLevel)
        {
            model = _context.Models.Include(x => x.Models).Include(x => x.Properties).FirstOrDefault(x => x.Id == model.Id);

            levels.Add(model.Name, currentLevel);

            var props = new ExpandoObject() as IDictionary<string, object>;


            if (model.Properties != null && model.Properties.Count > 0)
            {
                foreach (Property prop in model.Properties)
                {
                    string value;
                    //Data for this property will be generated here depending on the property data type
                    if (prop.Name == "FirstName") { value = "Thaddeus"; } else if (prop.Name == "LastName") { value = "Okafor"; } else { value = $"{prop.Name} data"; }

                    props.Add(prop.Name, value);
                }
            }

            if (model.Models != null && model.Models.Count > 0)
            {
                foreach (Model m in model.Models)
                {
                    currentLevel++;
                    props.Add(m.Name, BuildData(m, ref baseModel, ref levels, currentLevel));
                }
            }

            if (levels.ContainsKey(model.Name) && levels[model.Name] == 0)
            {
                baseModel.Add(model.Name, props);
            }

            return props;
        }
    }
}
