// Messenger custom code.
(function () {
    // Inject custom css in the page.
    var cssSetup = function() {
        if (!window.messengerCSS)
            return;

        // Create the style tag.
        var head = document.head || document.getElementsByTagName("head")[0];
        var style = document.createElement("style");
        style.type = "text/css";

        // Inject our style.
        if (style.styleSheet) {
            style.styleSheet.cssText = window.messengerCSS;
        } else {
            style.appendChild(document.createTextNode(window.messengerCSS));
        }

        // Add the tag to the page.
        head.appendChild(style);
    };

    // Make sure the persistent cookie checkbox is checked.
    var checkPersistent = function () {
        // Find the persistent checkbox.
        var persistentCheckbox = document.querySelector("input[name=persistent]");
        if (!persistentCheckbox)
            return;

        // Check it.
        persistentCheckbox.checked = true;
    };

    // Update the title of our wrapper.
    var updateTitle = function () {
        // Get a reference to the object containing the current title.
        var titleObject = document.querySelector("._5743 span");

        // If there's no interop bridge, or if a title couldn't be found, stop here.
        if (!window.messengerWrapper || !window.messengerWrapper.setTitle || !titleObject)
            return;

        // Try and extract the current activity name.
        var activityObject = document.querySelector("._2v6o");
        var activity = activityObject ? activityObject.textContent : "";

        // Notify the wrapper.
        window.messengerWrapper.setTitle(titleObject.textContent, activity);
    };

    // Update the taskbar badge count.
    var updateBadge = function () {
        // Get a reference to the page's title, and make sure we have an interop bridge with the wrapper.
        var titleObject = document.querySelector("title");
        if (!window.messengerWrapper || !window.messengerWrapper.updateBadge || !titleObject)
            return;

        // At this point, we should at least have the word "Messenger" in the title.
        var title = titleObject.textContent;
        var titlePrefix = "Messenger";
        if (title.indexOf(titlePrefix) < 0)
            return;

        // If there are no unread messages, notify our wrapper.
        if (title === titlePrefix) {
            window.messengerWrapper.updateBadge(0);
            return;
        }

        // Otherwise, extract the unread number from the title.
        var unreadCountStr = /\(([0-9^)]+)\)/.exec(title);
        var unreadCount = unreadCountStr && unreadCountStr.length > 1 ? parseInt(unreadCountStr[1]) : 0;

        window.messengerWrapper.updateBadge(unreadCount);
    };

    // Add functions to the window object for wrapper interop.
    window.messengerApi = {
        initialize: function() {
        }
    };

    // Inject our custom CSS.
    cssSetup();

    // Update the title and the badge every 200ms.
    setInterval(function () {
        updateTitle();
        updateBadge();
        checkPersistent();
    }, 200);
})();

// Google Analytics code.
(function (i, s, o, g, r, a, m) {
    i["GoogleAnalyticsObject"] = r; i[r] = i[r] || function () {
        (i[r].q = i[r].q || []).push(arguments);
    }, i[r].l = 1 * new Date(); a = s.createElement(o),
    m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g;
    m.parentNode.insertBefore(a, m);
})(window, document, "script", "//www.google-analytics.com/analytics.js", "ga");

ga("create", "UA-62755894-1", "auto");
ga("send", "pageview");