using Microsoft.AspNetCore.Components.Forms;

namespace ShellUI.Components.Models;

// What ChatInput sends: the trimmed text and the attached files. Read the files before the next message, and pass
// ChatInput.MaxFileSize to OpenReadStream, whose own default limit is 500 KB.
public record ChatSubmission(string Text, IReadOnlyList<IBrowserFile> Files);
