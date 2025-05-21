// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function captureScreenshot() {
    const canvas = document.querySelector("#3d-view-container canvas");
    if (!canvas) return null;
    return canvas.toDataURL('image/png');
}

async function saveStepScreenshot(stepNumber) {
    const screenshot = captureScreenshot();
    if (!screenshot) return;

    await fetch('/api/report/savescreenshot', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            schemeId: @Model.LoadingScheme.Id,
            stepNumber,
            data: screenshot
        })
    });
}