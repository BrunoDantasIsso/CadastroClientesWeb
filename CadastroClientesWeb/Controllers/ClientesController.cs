using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadastroClientesWeb.Context;
using CadastroClientesWeb.Models;

namespace CadastroClientesWeb.Controllers
{
    public class ClientesController : Controller
    {
        private readonly DataContext _context;

        public ClientesController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Endereco)
                .Include(c => c.Telefones)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null)
                if (cliente == null)
                {
                    return NotFound();
                }

            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Nome,Email,Endereco,Telefones")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Exibe os erros de validação no console/log
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(cliente);
            }
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                                .Include(c => c.Endereco)
                                .Include(c => c.Telefones)
                                .FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
                return NotFound();

            if (ModelState.IsValid)
            {


                var clienteExistente = await _context.Clientes
                    .Include(c => c.Endereco)
                    .Include(c => c.Telefones)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (clienteExistente == null)
                    return NotFound();

                // Atualiza dados básicos
                clienteExistente.Nome = cliente.Nome;
                clienteExistente.Email = cliente.Email;

                // Atualiza endereço
                if (clienteExistente.Endereco != null && cliente.Endereco != null)
                {
                    clienteExistente.Endereco.Logradouro = cliente.Endereco.Logradouro;
                    clienteExistente.Endereco.Numero = cliente.Endereco.Numero;
                    clienteExistente.Endereco.Bairro = cliente.Endereco.Bairro;
                    clienteExistente.Endereco.Cidade = cliente.Endereco.Cidade;
                    clienteExistente.Endereco.Estado = cliente.Endereco.Estado;
                    clienteExistente.Endereco.Cep = cliente.Endereco.Cep;
                }

                // Sincroniza telefones
                // Remove telefones que não vieram do form
                var idsForm = cliente.Telefones?.Where(t => t.Id != 0).Select(t => t.Id).ToList() ?? new List<int>();
                var telefonesRemover = clienteExistente.Telefones?.Where(t => !idsForm.Contains(t.Id)).ToList() ?? new List<Telefone>();
                foreach (var tel in telefonesRemover)
                    _context.Telefones.Remove(tel);

                // Atualiza ou adiciona telefones
                foreach (var telForm in cliente.Telefones ?? new List<Telefone>())
                {
                    if (telForm.Id == 0)
                    {
                        telForm.ClienteId = clienteExistente.Id;
                        _context.Telefones.Add(telForm);
                    }
                    else
                    {
                        var telDb = clienteExistente.Telefones?.FirstOrDefault(t => t.Id == telForm.Id);
                        if (telDb != null)
                            telDb.Numero = telForm.Numero;
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                Console.WriteLine(error.ErrorMessage);

            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
