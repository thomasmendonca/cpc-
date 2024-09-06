using Empresa.Models;
using Empresa.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Empresa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly IDepartamentoRepository _departamentoRepository;

        public DepartamentoController(IDepartamentoRepository departamentoRepository)
        {
            _departamentoRepository = departamentoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartamentos()
        {
            try
            {
                var departamentos = await _departamentoRepository.GetDepartamentos();
                return Ok(departamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao obter departamentos: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDepartamento(int id)
        {
            try
            {
                var departamento = await _departamentoRepository.GetDepartamento(id);
                if (departamento == null)
                    return NotFound($"Departamento com id = {id} não encontrado");

                return Ok(departamento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao obter o departamento: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartamento([FromBody] Departamento departamento)
        {
            try
            {
                if (departamento == null)
                    return BadRequest("Dados inválidos.");

                var novoDepartamento = await _departamentoRepository.AddDepartamento(departamento);
                return CreatedAtAction(nameof(GetDepartamento), new { id = novoDepartamento.DepId }, novoDepartamento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao criar o departamento: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDepartamento(int id, [FromBody] Departamento departamento)
        {
            try
            {
                var departamentoExistente = await _departamentoRepository.GetDepartamento(id);
                if (departamentoExistente == null)
                    return NotFound($"Departamento com id = {id} não encontrado");

                await _departamentoRepository.UpdateDepartamento(departamento);
                return Ok(departamento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar o departamento: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            try
            {
                var departamento = await _departamentoRepository.GetDepartamento(id);
                if (departamento == null)
                    return NotFound($"Departamento com id = {id} não encontrado");

                await _departamentoRepository.DeleteDepartamento(id);
                return Ok(departamento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar o departamento: {ex.Message}");
            }
        }

        [HttpGet("{depId:int}/Empregados")]
        public async Task<IActionResult> GetEmpregadosPorDepartamento(int depId)
        {
            try
            {
                var empregados = await _departamentoRepository.GetEmpregadosPorDepartamento(depId);
                if (empregados == null || !empregados.Any())
                    return NotFound("Nenhum empregado encontrado para este departamento.");

                return Ok(empregados);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao obter empregados do departamento: {ex.Message}");
            }
        }
    }
}
