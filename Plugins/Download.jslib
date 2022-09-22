mergeInto(LibraryManager.library, {
    Download: function (str, fn) {
        var msg = UTF8ToString(str);
        var fname = UTF8ToString(fn);
        function fixBinary(bin) {
            var length = bin.length;
            var buf = new ArrayBuffer(length);
            var arr = new Uint8Array(buf);
            for (var i = 0; i < length; i++) {
                arr[i] = bin.charCodeAt(i);
            }
            return buf;
        }
        var binary = fixBinary(atob(msg));
        var data = new Blob([binary]);
        var link = document.createElement('a');
        link.download = fname;
        link.href = URL.createObjectURL(data);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
});

