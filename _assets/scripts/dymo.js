
var dymoExtension = {

    GetPrinterName: function () {
        var printers = dymo.label.framework.getPrinters();
        if (printers.length == 0)
            throw "No DYMO printers are installed. Install DYMO printers.";

        var printerName = "";
        for (var i = 0; i < printers.length; ++i) {
            var printer = printers[i];
            if (printer.printerType == "LabelWriterPrinter") {
                printerName = printer.name;
                break;
            }
        }

        return printerName;
    },

    print: function (address) {

        var usexmlfile = "/_assets/media/label.xml";

        $.get(usexmlfile, function (data) {
            var label = dymo.label.framework.openLabelXml(data);
            label.setObjectText("Text", address);
            label.print(dymoExtension.GetPrinterName());
        }, "text");

    }

}