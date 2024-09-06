using Empresa.Models;
using Empresa.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Empresa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpregadoController : ControllerBase
    {
        private readonly IEmpregadoRepository _empregadoRepository;

        public EmpregadoController(IEmpregadoRepository empregadoRepository)
        {
            _empregadoRepository = empregadoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpregados()
        {
            try
            {
                var empregados = await _empregadoRepository.GetEmpregados();
                return Ok(empregados);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao recuperar empregados: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmpregado(int id)
        {
            try
            {
                var empregado = await _empregadoRepository.GetEmpregado(id);
                if (empregado == null)
                    return NotFound($"Empregado com id = {id} não encontrado");

                return Ok(empregado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao recuperar empregado: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmpregado([FromBody] Empregado empregado)
        {
            try
            {
                if (empregado == null)
                    return BadRequest("Dados inválidos.");

                var novoEmpregado = await _empregadoRepository.AddEmpregado(empregado);
                return CreatedAtAction(nameof(GetEmpregado), new { id = novoEmpregado.EmpId }, novoEmpregado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao adicionar empregado: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmpregado(int id, [FromBody] Empregado empregado)
        {
            try
            {
                var empregadoExistente = await _empregadoRepository.GetEmpregado(id);
                if (empregadoExistente == null)
                    return NotFound($"Empregado com id = {id} não encontrado");

                await _empregadoRepository.UpdateEmpregado(empregado);
                return Ok(empregado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar empregado: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmpregado(int id)
        {
            try
            {
                var empregado = await _empregadoRepository.GetEmpregado(id);
                if (empregado == null)
                    return NotFound($"Empregado com id = {id} não encontrado");

                await _empregadoRepository.DeleteEmpregado(id);
                return Ok(empregado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar empregado: {ex.Message}");
            }
        }

        [HttpPost("AssociarEmpregado")]
        public async Task<IActionResult> AssociarEmpregadoAoDepartamento(int empId, int depId)
        {
            try
            {
                var associado = await _empregadoRepository.AssociarEmpregadoAoDepartamento(empId, depId);
                if (associado == null)
                    return NotFound($"Falha ao associar o empregado com id = {empId} ao departamento com id = {depId}");

                return Ok(associado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao associar empregado ao departamento: {ex.Message}");
            }
        }
    }
}
