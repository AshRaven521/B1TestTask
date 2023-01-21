using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Task2.Model.Files;
using Task2.Service;
using Task2.Utils;

namespace Task2.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("get-file-by-id")]
        public async Task<IActionResult> GetFileById(int fileId)
        {
            if (fileId < 1)
            {
                return BadRequest();
            }

            var file = await fileService.GetFileByIdAsync(fileId);
            return Ok(file.FileData);
        }

        [HttpPost("post-single-file")]
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

            string newPath = FileUtil.MapPath(file.FileDetails.FileName);
            using (var stream = new MemoryStream())
            {
                file.FileDetails.CopyTo(stream);
                FileUtil.FillFile(newPath, stream.ToArray());
            }

            var (balanceList, csList) = ExcelUtil.FillModels(newPath, addedFileId);
            try
            {
                await balanceService.InsertBalancesAsync(balanceList);
                await csService.InsertCustomStringsAsync(csList);
            }
            catch(Exception)
            {
                throw;
            }

            return Ok();
        }

        [HttpPost("post-multiple-file")]
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
