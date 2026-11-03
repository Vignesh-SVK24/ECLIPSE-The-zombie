// ============================================================================
// ECLIPSE: THE LAST CIVILIZATION
// Adobe Photoshop to Unity 6 URP Automated Texture Exporter (ExtendScript JSX)
// ============================================================================

#target photoshop

function exportTextureLayers() {
    if (!app.documents.length) {
        alert("No active Photoshop document found.");
        return;
    }

    var doc = app.activeDocument;
    var docName = doc.name.replace(/\.[^\.]+$/, '');

    // Target Unity project directory
    var unityRoot = "C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/ECLIPSE/Textures/";
    var exportFolder = new Folder(unityRoot);

    if (!exportFolder.exists) {
        exportFolder.create();
    }

    // Configure PNG Save Options
    var pngOptions = new PNGSaveOptions();
    pngOptions.compression = 6;
    pngOptions.interlaced = false;

    // Check layer groups for export
    for (var i = 0; i < doc.layerSets.length; i++) {
        var group = doc.layerSets[i];
        if (group.name.indexOf("EXPORT_") === 0) {
            var suffix = group.name.replace("EXPORT_", "");
            
            // Isolate current layer group
            isolateLayerGroup(doc, group);

            var outputFile = new File(unityRoot + docName + "_" + suffix + ".png");
            doc.saveAs(outputFile, pngOptions, true, Extension.LOWERCASE);
            $.writeln("Exported: " + outputFile.fsName);
        }
    }

    alert("ECLIPSE Textures successfully exported to Unity project:\n" + unityRoot);
}

function isolateLayerGroup(doc, targetGroup) {
    for (var i = 0; i < doc.layerSets.length; i++) {
        var grp = doc.layerSets[i];
        grp.visible = (grp === targetGroup);
    }
}

exportTextureLayers();
