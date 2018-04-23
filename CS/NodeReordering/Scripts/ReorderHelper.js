function ReorderHelper(treeList, reorderImageUrl, insertImageUrl) {
    var self = this;
    
    self.reorderImageUrl = reorderImageUrl;
    self.insertImageUrl = insertImageUrl;

    self.treeListReorderTargets = [];
    self.hasMouseMoveEvent = false;
    self.treeList = treeList;

    self.targetRects = {};

    var LoadTargetsPos = function (targets) {
        for (var i = 0; i < targets.length; i++) {
            var target = targets[i];
            var x = ASPx.GetAbsoluteX(target);
            var y = ASPx.GetAbsoluteY(target);
            var rect = {
                top: y,
                right: x + target.offsetWidth,
                bottom: y + target.offsetHeight,
                left: x
            };
            self.targetRects[target.id] = rect;
        }
    }

    var GetHoveredTargets = function (x, y) {
        return self.treeListReorderTargets.filter(function (target) {
            var rect = self.targetRects[target.id];
            return rect.left <= x && rect.right >= x && rect.top <= y && rect.bottom >= y;
        });
    }

    var GetTargetNodeKey = function (treeList, target) {
        var regexp = new RegExp("^" + treeList.name + "_R-(\\w+)$");
        var matches = regexp.exec(target.id);
        return matches && matches.length === 2 ? matches[1] : null;
    }

    var ChangeDragImage = function (reorder) {
        var img = self.treeList.GetDragAndDropNodeImage();

        if (!img.insertInfo)
            img.insertInfo = { src: self.insertImageUrl, className: "" };
        if (!img.reorderInfo)
            img.reorderInfo = { src: self.reorderImageUrl, className: "" };

        var info = reorder ? img.reorderInfo : img.insertInfo;

        img.src = info.src;
        img.className = info.className;
    }

    var CanReorderNodes = function (e) {
        var x = ASPx.Evt.GetEventX(e);
        var y = ASPx.Evt.GetEventY(e);
        var hoveredTargets = GetHoveredTargets(x, y);
        if (hoveredTargets.length === 0)
            return;
        var target = hoveredTargets[0];
        var rect = self.targetRects[target.id];
        var targetHeight = rect.bottom - rect.top;

        var result = y < rect.top + targetHeight / 3;

        return result;
    }

    var OnDocMouseMove = function (e) {
        if (self.treeListReorderTargets.length === 0)
            return;
        ChangeDragImage(CanReorderNodes(e));
    }

    var EnsureMouseMoveEvent = function () {
        if (self.hasMouseMoveEvent) return;
        ASPx.Evt.AttachEventToDocument("mousemove", OnDocMouseMove);
    }

    var tree_StartDragNode = function (s, e) {
        EnsureMouseMoveEvent();

        var siblingKeys = s.cpSiblingKeys[e.nodeKey];

        // Reorder on the same level only
        self.treeListReorderTargets = e.targets.filter(function (target) { return siblingKeys.indexOf(GetTargetNodeKey(s, target)) > -1; });

        LoadTargetsPos(self.treeListReorderTargets);
    };

    var tree_EndDragNode = function (s, e) {
        var canReorder = CanReorderNodes(e.htmlEvent);
        if (canReorder) {
            var x = ASPx.Evt.GetEventX(e.htmlEvent);
            var y = ASPx.Evt.GetEventY(e.htmlEvent);
            var hoveredTargets = GetHoveredTargets(x, y);
            var reorderNodeKey = GetTargetNodeKey(s, hoveredTargets[0]);

            e.cancel = true;
            s.PerformCallback(e.nodeKey + '|' + reorderNodeKey + '|' + canReorder);
        }

        self.treeListReorderTargets = [];
        self.targetRects = {};
        ChangeDragImage(false);
    };


    if (self.treeList) {
        self.treeList.StartDragNode.ClearHandlers();
        self.treeList.EndDragNode.ClearHandlers();

        self.treeList.StartDragNode.AddHandler(tree_StartDragNode);
        self.treeList.EndDragNode.AddHandler(tree_EndDragNode);
    }
}