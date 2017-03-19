using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPCoreCustomModelBinder.Models;

namespace ASPCoreCustomModelBinder.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly DataContext _context;

        public AppointmentsController(DataContext context)
        {
            _context = context;    
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Appointment.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/CreateForm
        public IActionResult CreateForm()
        {
            return View();
        }

        // POST: Appointments/CreateForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForm([Bind("Name,AppointmentDate")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.Id = Guid.NewGuid().ToString();
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        // GET: Appointments/CreateJson
        public IActionResult CreateJson()
        {
            return View();
        }

        // POST: Appointments/CreateJson
        [HttpPost]
        public async Task<IActionResult> CreateJson([FromBody]Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.Id = Guid.NewGuid().ToString();
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return Json(appointment);
            }
            return StatusCode(400);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.SingleOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, Name,AppointmentDate")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var appointment = await _context.Appointment.SingleOrDefaultAsync(m => m.Id == id);
            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AppointmentExists(string id)
        {
            return _context.Appointment.Any(e => e.Id == id);
        }
    }
}
