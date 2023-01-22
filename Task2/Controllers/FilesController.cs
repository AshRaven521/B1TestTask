using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task2.Model.Files;
using Task2.Service;
using Task2.Utils;

namespace Task2.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private IFileService fileService;
        private IBalanceService balanceService;
        private ICustomStringService csService;

        public FilesController(IFileService fileService, IBalanceService balanceService, ICustomStringService csService)
        {
            this.fileService = fileService;
            this.balanceService = balanceService;
            this.csService = csService;
        }

        [HttpGet("files-names")]
        public async Task<IActionResult> GetFilesNames()
        {
            var files = await fileService.GetFileNames();
            return Ok(files);
        }

        [HttpGet("file-by-id")]
        public async Task<IActionResult> GetFileById(int fileId)
        {
            if (fileId < 1)
            {
                return BadRequest();
            }

            var file = await fileService.GetFileByIdAsync(fileId);
            var balances = balanceService.GetBalancesByFileId(fileId);
            return Ok(balances);
        }

        [HttpPost("single-file")]
        public async Task<IActionResult> PostSingleFile([FromForm] FileUploaded file)
        {
            if (file == null)
            {
                return BadRequest();
            }
            int addedFileId = 0;
            try
            {
                addedFileId = await fileService.PostFileAsync(file.FileDetails, file.FileType);
            }
            catch (Exception)
            {
                throw;
            }

            var fileStream = file.FileDetails.OpenReadStream();
            var (balanceList, csList) = ExcelUtil.FillModels(fileStream, addedFileId);

            try
            {
                await balanceService.InsertBalancesAsync(balanceList);
                await csService.InsertCustomStringsAsync(csList);
            }
            catch (Exception)
            {
                throw;
            }

            var simple = new SimpleFile
            {
                Id = addedFileId,
                FileName = file.FileDetails.FileName
            };

            return Ok(simple);
        }

        [HttpPost("multiple-files")]
        public async Task<IActionResult> PostMultipleFile([FromForm] List<FileUploaded> fileDetails)
        {
            if (fileDetails == null)
            {
                return BadRequest();
            }

            try
            {
                await fileService.PostMultipleFileAsync(fileDetails);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("download-file")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            try
            {
                await fileService.DownloadFileById(id);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
