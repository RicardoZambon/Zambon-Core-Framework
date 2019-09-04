class Drawer {
    constructor(buttonId, sidebarContainerId, sidebarId) {
        var button = document.getElementById(buttonId);

        this.sidebarContainerId = sidebarContainerId;
        this.sidebarId = sidebarId;

        button.addEventListener('click', this.drawerCollapse.bind(this));

        var sidebar = document.getElementById(sidebarId);
        var items = sidebar.getElementsByClassName('nav-item');
        if (items.length > 0) {
            this.activeItems = [items[0]];
            this.activeItems[0].classList.add('active');
        }

        for (var i = 0; i < items.length; i++) {
            items[i].getElementsByClassName('nav-link')[0].addEventListener('click', this.itemNavigate.bind(this));

            if (items[i].getElementsByTagName('ul').length > 0) {
                items[i].classList.add('closed');
            }
        }
        document.addEventListener('click', this.hideFloatingSubMenu.bind(this), false);
    }

    drawerCollapse(event) {
        var container = document.getElementById(this.sidebarContainerId);
        if (container.classList.contains('drawer-expanded')) {
            container.classList.remove('drawer-expanded');
            container.classList.add('drawer-fade-out-text');
            container.classList.add('drawer-collapsing');
            setTimeout(function () {
                container.classList.add('drawer-collapse-items');
                container.classList.remove('drawer-fade-out-text');
                setTimeout(function () {
                    container.classList.remove('drawer-collapse-items');
                    container.classList.remove('drawer-collapsing');
                }, 250);
                container.classList.add('drawer-collapsed');
            }, 125);
        }
        else {
            this.hideFloatingSubMenu(event);

            var hideFloatingMenusTimeOut = 0;
            var openMenus = document.getElementsByClassName('float-menu');
            if (openMenus.length > 0) {
                hideFloatingMenusTimeOut = 250;
                openMenus[0].classList.remove('float-menu');
            }

            setTimeout(function () {
                container.classList.add('drawer-expanding');
                container.classList.remove('drawer-collapsed');
                setTimeout(function () {
                    container.classList.add('drawer-fade-in-text');
                    container.classList.remove('drawer-expanding');
                    setTimeout(function () {
                        container.classList.remove('drawer-fade-in-text');
                    }, 125);
                    container.classList.add('drawer-expanded');
                }, 125);
            }, hideFloatingMenusTimeOut);
        }
        event.stopPropagation();
    }

    itemNavigate(event) {
        event.stopPropagation();

        var currentTargetItem = event.currentTarget.parentNode;

        var currentLevel = parseInt(event.currentTarget.parentNode.parentNode.getAttribute("data-level"));
        var openedLevel = parseInt(this.activeItems.length - 1);

        if (currentLevel == openedLevel) {
            //Same menu level
            var activeItem = this.activeItems[currentLevel];
            if (activeItem != currentTargetItem) {
                activeItem.classList.remove('active');
                if (activeItem.classList.contains('opened')) {
                    this.collapseParentMenu(activeItem, true);
                }
            }
            this.activeItems[currentLevel] = currentTargetItem;

        } else if (currentLevel > openedLevel) {
            //Higher menu level
            this.activeItems.push(currentTargetItem);

        } else {
            //Lower menu level
            for (var i = currentLevel; i < this.activeItems.length; i++) {
                this.activeItems[i].classList.remove('active');
                if (this.activeItems[i] != currentTargetItem) {
                    this.collapseParentMenu(this.activeItems[i], true);
                }
            }
            this.activeItems.length = currentLevel;
            this.activeItems.push(currentTargetItem);
        }

        //Set current item active
        currentTargetItem.classList.add('active');
        if (currentTargetItem.getElementsByTagName('ul').length > 0) {
            var container = document.getElementById(this.sidebarContainerId);
            if (currentLevel == 0 && container.classList.contains('drawer-collapsed')) {
                if (currentTargetItem.classList.contains('opened') && !currentTargetItem.getElementsByTagName('ul')[0].classList.contains('float-menu')) {
                    this.showFloatingSubMenu(currentTargetItem);
                } else {
                    this.collapseParentMenu(currentTargetItem, false);

                    if (currentTargetItem.getElementsByTagName('ul')[0].classList.contains('float-menu')) {
                        this.hideFloatingSubMenu(event);
                    } else {
                        this.hideFloatingSubMenu(event);
                        this.showFloatingSubMenu(currentTargetItem);
                    }
                }
            }
            else {
                this.collapseParentMenu(currentTargetItem, false);
            }
        } else if (currentLevel == 0) {
            this.hideFloatingSubMenu(event);
        }

    }

    collapseParentMenu(menuItem, closeOnly) {
        if (menuItem.getElementsByTagName('ul').length > 0) {
            if (menuItem.classList.contains('opened')) {
                menuItem.classList.remove('opened');
                menuItem.classList.add('closed');
            } else if (!closeOnly) {
                var maxHeight = 0;
                var childMenus = menuItem.getElementsByTagName('li');
                for (var i = 0; i < childMenus.length; i++) {
                    maxHeight += childMenus[i].offsetHeight;
                }

                menuItem.classList.remove('closed');
                menuItem.classList.add('opened');
                menuItem.getElementsByTagName('ul')[0].style.maxHeight = maxHeight + 'px';
            }
        }
    }

    showFloatingSubMenu(menuItem) {
        var container = document.getElementById(this.sidebarContainerId);
        if (container.classList.contains('drawer-collapsed')) {
            var ul = menuItem.getElementsByTagName('ul')[0];
            var sidebar = document.getElementById(this.sidebarId);

            ul.style.maxHeight = "auto !important";
            ul.style.top = (menuItem.getBoundingClientRect().top - document.body.getBoundingClientRect().top) + 'px';
            ul.style.left = (document.body.getBoundingClientRect().left + sidebar.getElementsByClassName("nav")[0].getBoundingClientRect().width - ul.getBoundingClientRect().width) + 'px';
            ul.classList.add('float-menu');
        }
    }

    hideFloatingSubMenu(event) {
        try {
            if (!event.currentTarget.classList.contains('collapse-button')) {
                var openMenus = document.getElementsByClassName('float-menu');
                if (openMenus.length > 0) {
                    var menu = openMenus[0];
                    menu.classList.remove('float-menu');
                }
            }
        }
        catch { }
    }

}