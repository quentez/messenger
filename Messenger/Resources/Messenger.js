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
    var lastTitle = null;
    var updateTitle = function () {
        // Get a reference to the object containing the current title.
        var titleObject = document.querySelector("._5743 span");

        // If there's no interop bridge, or if a title couldn't be found, stop here.
        if (!window.messengerWrapper || !window.messengerWrapper.updateTitle || !titleObject || titleObject.textContent === lastTitle)
            return;
        
        // Notify the wrapper.
        lastTitle = titleObject.textContent;
        window.messengerWrapper.updateTitle(titleObject.textContent);
    };

    // Helper method to notify the wrapper about the updated badge.
    var lastBadge = null;
    var notifyBadge = function(newBadge) {
        if (!window.messengerWrapper || !window.messengerWrapper.updateBadge || newBadge === lastBadge)
            return;

        lastBadge = newBadge;
        window.messengerWrapper.updateBadge(newBadge);
    };

    // Act on unread conversations.
    var lastNotification = null;
    var updateUnread = function () {
        // Detect the number of conversations with unread messages.
        var unreadConversations = document.querySelectorAll("._1ht3");

        // Update the badge accordingly.
        notifyBadge(unreadConversations.length);

        // If we have more than one, take the first one and trigger a notification.
        if (unreadConversations.length === 0 || !window.messengerWrapper || !window.messengerWrapper.showNotification) {
            lastNotification = "";
            return;
        }

        var firstUnreadConversation = unreadConversations[0];

        // Find the title.
        var titleObject = firstUnreadConversation.querySelector("._1ht6");
        if (!titleObject)
            return;

        var title = titleObject.textContent;

        // Find the subtitle.
        var subtitleObject = firstUnreadConversation.querySelector("._1htf");
        var subTitle = subtitleObject ? subtitleObject.textContent : "";

        // Find the conversation id.
        var idObject = document.querySelector("._1ht1._1ht3");
        var id = idObject ? idObject.getAttribute("data-reactid") : "";

        // Check if we already notified for this.
        var signature = title + subTitle + id;
        if (signature === lastNotification)
            return;

        // If this is the first run, don't send an actual notification.
        var firstRun = lastNotification === null;

        // Update the last signature and call the Api.
        lastNotification = signature;

        if (firstRun)
            return;

        window.messengerWrapper.showNotification(title, subTitle, id);
    };

    // Add functions to the window object for wrapper interop.
    window.messengerApi = {
        selectConversation: function (id) {
            // Find the corresponding conversation list item.
            var conversationObject = document.querySelector("[data-reactid=\"" + id + "\"] a");
            if (!conversationObject)
                return;

            // Simulate a click on the corresponding conversation.
            conversationObject.click();
        }
    };

    // Inject our custom CSS.
    cssSetup();

    // Update the title and the badge every 200ms.
    setInterval(function () {
        updateTitle();
        updateUnread();
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