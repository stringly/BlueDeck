﻿

    // Drag and Drop
    var dragSrcEl = null;
    var dragSrcParentComponentId = null;
    function handleDragStart(e) {
        // Target (this) element is the source node.
        dragSrcEl = this;
        dragSrcParentComponentId = this.parentElement.dataset.parentcomponentid;
        e.dataTransfer.effectAllowed = 'move';
        e.dataTransfer.setData('text', this.outerHTML);
        this.classList.add('dragElem');
        var dragGhost = document.createElement("div")
        dragGhost.className = "dragGhost-member";
        dragGhost.style.width = "" + this.offsetWidth + "px";
        dragGhost.innerHTML = this.innerHTML;
        dragGhost.style.opacity = 1;
        document.body.appendChild(dragGhost);
        var posX = $(this).offset().left, posY = $(this).offset().top;
        e.dataTransfer.setDragImage(dragGhost, (e.pageX - posX), (e.pageY - posY));
    }

    function handleDragOver(e) {
        
        if (e.preventDefault) {
            e.preventDefault(); // Necessary. Allows us to drop.
        }
        this.classList.add('over');
        if (this.classList.contains('componentHeaderMenuItem')) {
            var componentid = this.dataset.componentid;
            if ($("#componentDetailSection" + componentid).is(":hidden")) {
                handleComponentShowHide(e);
                //$("#componentDetailSection" + componentid).show();
            }
        }

        e.dataTransfer.dropEffect = 'move';  // See the section on the DataTransfer object.
        return false;
    }
    function handleDragEnter(e) {
        // this / e.target is the current hover target.

    }
    function handleDragLeave(e) {

        this.classList.remove('over');  // this / e.target is previous target element.
    }
    function handleDrop(e) {
        // this/e.target is current target element.
        if (e.stopPropagation) {
            e.stopPropagation(); // Stops some browsers from redirecting.
        }
        // Don't do anything if dropping the same element we're dragging.
        if (dragSrcEl != this) {
            // ensure the target is droppable by checking for the ".insertable" class
            var droppable = false;
            var swappable = false;
            for (var i = 0; i < this.classList.length; i++) {
                if (this.classList[i] == "insertable") {
                    // i need to address the drop targeting... it appears possible that 
                    // the "drop" event can fire on an insertable even if the insertable has a member, so
                    // i need to make sure that the insertable doesn't have members... if it does, i need to swap, not drop
                    for (var i = 0; i < this.childNodes.length; i++) {
                        if (this.childNodes[i].classList) {
                            if (this.childNodes[i].classList.contains("member")) {
                                swappable = true;
                            }
                        }
                    }
                    droppable = true;
                }
                else if (this.classList[i] == "member") {
                    swappable = true;
                }
            }
            // exit the function if target is not an ".insertable" class
            if (!droppable && !swappable) { return; }
            //  check whether we are dropping or swapping
            if (swappable) {
                // target element IS a member - we need to swap
                // swapping invokes a Roster refresh, so we return after this                        
                var dragMemberId = dragSrcEl.dataset.memberid;
                var dropMemberId = this.dataset.memberid;
                if (dropMemberId == null) {
                    // if the drop event fired on the "insertable", the memberId will be in the dataset of it's first class="member" child
                    dropMemberId = this.getElementsByClassName("member")[0].dataset.memberid;
                }
                DragAndSwap(dragMemberId, dropMemberId);
                return;
            }

            // We wont get here unless we are dropping instead of swapping

            // Remove all current children of target insertable
            while (this.hasChildNodes()) {
                this.removeChild(this.lastChild);
            }
            // Now, we are ready to drop

            // We need the original position's id to set the data-positionid attribute on the
            // class=".insertableInner" element we will add into the emptied position
            var originalPositionId = dragSrcEl.parentElement.dataset.positionid;
            // insert a "insertableInner" element into the vacated position
            // setting the parentNode's innerHTML in this manner also clears the dragSrcEl from the parent
            var oldParentNode = dragSrcEl.parentNode;
            while (oldParentNode.hasChildNodes()) {
                oldParentNode.removeChild(oldParentNode.lastChild);
            }
            var newDiv = document.createElement("div");
            newDiv.dataset.positionid = originalPositionId;
            newDiv.classList.add("insertableInner");
            newDiv.textContent = "Drag & Drop or ";
            var newSpan = document.createElement("span");
            newSpan.classList.add("insertableInnerSelectMember");
            newSpan.dataset.positionid = originalPositionId;
            newSpan.addEventListener('click', HandleAssignMemberClick, false);
            newSpan.textContent = "click to select member.";
            newDiv.appendChild(newSpan);
            oldParentNode.appendChild(newDiv);

            // Set the source column's HTML to the HTML of the column we dropped on.
            var dropHTML = e.dataTransfer.getData('text');
            this.insertAdjacentHTML('beforeend', dropHTML);

            var dropElem = document.getElementById(this.id).lastChild;
            addDnDHandlers(dropElem);
            var moreOptionsButton = dropElem.querySelectorAll('.moreOptions')[0];
            addShowMoreOptionsHandler(moreOptionsButton);
            var moreOptionsReassignEmployee = dropElem.querySelectorAll('.drag-options-reassign')[0];
            addMoreOptionsReassignEmployeeHandler(moreOptionsReassignEmployee);
            var moreOptionsEditMember = dropElem.querySelectorAll('.drag-options-edit')[0];
            addMoreOptionsEditMemberHandler(moreOptionsEditMember);
            var moreOptionsChangeDutyStatus = dropElem.querySelectorAll('.drag-options-changestatus')[0];
            addMoreOptionsChangeDutyStatusHandler(moreOptionsChangeDutyStatus);
            // find: MemberID of dragged member, PositionID of new position.
            var newPositionId = this.dataset.positionid;
            var memberId = dragSrcEl.dataset.memberid;
            reassignMember(memberId, newPositionId);
        }
        this.classList.remove('over');
        return false;
    }
    function handleDragEnd(e) {
        // this/e.target is the source node.
        this.classList.remove('over');
        var ghosts = document.getElementsByClassName("dragGhost-member");
        [].forEach.call(ghosts, function (elem) {
            elem.parentNode.removeChild(elem);
        })
    }
    function addDnDHandlers(elem) {
        console.log("Adding handlers to: " + elem);
        elem.addEventListener('dragstart', handleDragStart, false);
        elem.addEventListener('dragenter', handleDragEnter, false)
        elem.addEventListener('dragover', handleDragOver, false);
        elem.addEventListener('dragleave', handleDragLeave, false);
        elem.addEventListener('drop', handleDrop, false);
        elem.addEventListener('dragend', handleDragEnd, false);
    }
    var cols = document.querySelectorAll('.members .member');
    [].forEach.call(cols, addDnDHandlers);

    // attempt drag scroll
    var stop = true;
    $(".draggable").on("drag", function (e) {
        stop = true;
        if (e.originalEvent.clientY < 150) {
            stop = false;
            scroll(-1)
        }
        if (e.originalEvent.clientY > ($(window).height() - 150)) {
            stop = false;
            scroll(1)
        }
    });

    $(".draggable").on("dragend", function (e) {
        stop = true;
    });
    var scroll = function (step) {
        var scrollY = $(window).scrollTop();
        $(window).scrollTop(scrollY + step);
        if (!stop) {
            setTimeout(function () { scroll(step) }, 20);
        }
    }
    function addShowMoreOptionsHandler(elem) {
        elem.addEventListener('click', showMoreOptions, false);
        elem.parentElement.parentElement.querySelectorAll(".drag-options")[0].addEventListener('mouseleave', handleMouseOut, false);
    }
    function addMoreOptionsReassignEmployeeHandler(elem) {
        elem.addEventListener('click', handleMoreOptionsReassignEmployeeClick, false);
    }
    function addMoreOptionsChangeDutyStatusHandler(elem) {
        elem.addEventListener('click', handleMoreOptionsChangeDutyStatusClick, false);
    }
    function addMoreOptionsEditMemberHandler(elem) {
        elem.addEventListener('click', handleMoreOptionsEditMemberClick, false);
    }
    function showMoreOptions(e) {
        var memberId = $(e.target).data('memberid');
        console.log('More options clicked for Member: ' + memberId);
        $("#options_" + memberId).toggleClass("active");
    }
    function handleMouseOut(elem) {
        this.classList.remove("active");
    }
    function DragAndSwap(dragMId, dropMId) {
        $.get("Roster/SwapMemberPositions", { dragMemberId: dragMId, dropMemberId: dropMId }, function (data) {

            if (data.success == true) {
                $("#getComponentButton").trigger('click');
            }
        })
    }
    function reassignMember(memberId, newPositionId) {
        $.get("/Roster/ReassignMember", { memberId: memberId, positionId: newPositionId, selectedComponentId: $("#Component").val() }, function (data) {
            // check if a DemoRefresh has been returned
            if (data.demoDictionary) {
                console.log("Refreshing DemoTable");
                for (var key in data.demoDictionary) {
                    // skip loop if the property is from prototype
                    if (!data.demoDictionary.hasOwnProperty(key)) continue;
                    var obj = data.demoDictionary[key];
                    $(key).html(obj);
                }
            }
                // check if we need to refresh the entire RosterManager
            else if (data.status == "RefreshRosterManager") {
                // if so, manually trigger the click event for the getComponentButton
                console.log("Refreshing RosterManager");
                $("#getComponentButton").trigger('click');
            }
        });
    }
    function handleMoreOptionsReassignEmployeeClick(e) {
        // retrieve the Member that we want to transfer
        var memberId = $(e.target).data('memberid');
        // clear any extant modal from the modal container
        var container = $("#modalTarget").empty();
        // retrieve the ReassignEmployee from the server
        $.get("Roster/GetReassignMemberViewComponent", { memberId: memberId }, function (data) {
            // inject the ViewComponent HTML into the modal container
            container.html(data);
            // register the Modal's event handlers
            var submitButton = document.getElementById("reassignEmployeeModalSubmit");
            submitButton.addEventListener('click', handleReassignEmployeeModalSubmit, false);
            $("#reassignEmployeeModal").modal("show");
        })
    }

    function handleAddNewPositionButtonClick(e) {
        // retrieve the Component to which we want to add a position
        var componentId = e.target.dataset.componentid;
        // clear any extant modal from the modal container
        var container = $("#modalTarget").empty();
        // retrieve the AddPositionModal from the server
        $.get('Roster/GetAddPositionToComponentViewComponent', { componentId: componentId }, function (data) {
            // inject the ViewComponent HTML into the modal container
            container.html(data);
            // register the Modal's event handlers
            var submitButton = document.getElementById("addPositionToComponentModalSubmit");
            submitButton.addEventListener('click', handleAddPositionToComponentModalSubmit, false);
            var parentComponentSelect = document.getElementById('ParentComponentId');
            parentComponentSelect.addEventListener('change', handleParentComponentChange, false);
            var isManagerCheckBox = document.getElementById('IsManager');
            isManagerCheckBox.addEventListener('change', handleIsManagerChange, false);
            $.get("/Positions/GetPositionLineupViewComponent", { componentId: $("#ParentComponentId").val() }, function (data) {
                $("#lineupSelectionTarget").html(data);
                document.getElementById('lineupMoveUp').addEventListener('click', handleLineupMoveUp, false);
                document.getElementById('lineupMoveDown').addEventListener('click', handleLineupMoveDown, false);
            });
            // show the modal
            $("#addPositionToComponentModal").modal("show");
        })
    }

    function handleShowDeletePositionConfirm() {
        $("#deletePositionSubModal").modal("show");
    }

    function handleMoreOptionsEditMemberClick(e) {
        // retrieve the MemberId of the member we want to edit
        var member = e.target.dataset.memberid;
        // clear any existing modals from the container
        var container = $("#modalTarget").empty();
        // retrieve the EditEmployeeModal from the server
        $.get('Roster/GetEditEmployeeModalViewComponent', { memberId: member }, function (data) {
            // inject the ViewComponent HTML into the modal container
            container.html(data);
            // register the Modal's event handlers
            var submitButton = document.getElementById("editMemberModalSubmit");
            submitButton.addEventListener('click', handleEditEmployeeSubmitButtonClick, false);
            // show the modal
            $("#editMemberModal").modal("show");
        })
    }
    function handleMoreOptionsChangeDutyStatusClick(e) {
        // retrive the MemberId of the member we want to edit
        var member = e.target.dataset.memberid;
        // clear any existing modals from the container
        var container = $("#modalTarget").empty();
        // retrieve the ChangeDutyStatus modal from the server
        $.get('Roster/GetChangeEmployeeStatusModalViewComponent', { memberId: member }, function (data) {
            // inject the ViewComponent HTML into the container
            container.html(data);
            // register the Modal's control's event handlers
            var submitButton = document.getElementById('changeEmployeeStatusModalSubmit');
            submitButton.addEventListener('click', handleChangeDutyStatusSubmitButtonClick, false);
            // show the modal
            $("#changeEmployeeStatusModal").modal("show");
        })
    }
    /*
     * Edit a Position 
     * 
     */

    // Retrieve Edit Position Modal
    function handleEditPositionButtonClick(e) {
        // retrieve the PositionId of the position we want to edit
        var position = e.target.dataset.positionid;
        // clear any modals from the container
        var container = $("#modalTarget").empty();
        // retrieve the EditPositionModal from the server
        $.get('Roster/GetAddPositionToComponentViewComponent', { componentId: '0', positionId: position }, function (data) {
            // inject the viewcomponent HTML into the container
            container.html(data);
            // register the modal's submit button handler
            var submitButton = document.getElementById('addPositionToComponentModalSubmit');
            submitButton.addEventListener('click', handleAddPositionToComponentModalSubmit, false);
            var showDeleteButton = document.getElementById("deletePositionModalButton");
            showDeleteButton.addEventListener('click', handleShowDeletePositionConfirm, false);
            var deletePositionConfirmButton = document.getElementById("deletePositionConfirm");
            deletePositionConfirmButton.addEventListener('click', handleDeletePosition, false);
            var parentComponentSelect = document.getElementById('ParentComponentId');
            parentComponentSelect.addEventListener('change', handleParentComponentChange, false);
            var isManagerCheckBox = document.getElementById('IsManager');
            isManagerCheckBox.addEventListener('change', handleIsManagerChange, false);
            $.get("/Positions/GetPositionLineupViewComponent", { componentId: $("#ParentComponentId").val(), positionBeingEditedId: $("#PositionId").val() }, function (data) {
                $("#lineupSelectionTarget").html(data);
                document.getElementById('lineupMoveUp').addEventListener('click', handleLineupMoveUp, false);
                document.getElementById('lineupMoveDown').addEventListener('click', handleLineupMoveDown, false);
            });
            // show the modal
            $("#addPositionToComponentModal").modal("show");
        })
    }

   
    function handleParentComponentChange(e) {
        if (this.value != null && !document.getElementById('IsManager').checked) {
            // retrieve the Lineup Interface view Component
            var container = $("#lineupSelectionTarget").empty();
            $.get("/Positions/GetPositionLineupViewComponent", { componentId: this.value, positionBeingEditedId: $("#PositionId").val() }, function (data) {
                container.html(data);
                document.getElementById('lineupMoveUp').addEventListener('click', handleLineupMoveUp, false);
                document.getElementById('lineupMoveDown').addEventListener('click', handleLineupMoveDown, false);
                $("#LineupPosition").val($("#newPositionInsert").index());
            });
        }
    }


    function handleIsManagerChange(e) {
        console.log("ParentComponent value is: " + $("#ParentComponentId").val());
        console.log("LineupBox isEmpty? " + $("#lineupSelectionTarget").is(':empty'));
        if (this.checked) {
            $("#lineupSelectionTarget").empty();
        }
        else if ($("#ParentComponentId").val() != null && $("#lineupSelectionTarget").is(':empty')) {
            var container = $("#lineupSelectionTarget").empty();
            console.log("Attempting to repoll ViewComponent");
            $.get("/Positions/GetPositionLineupViewComponent", { componentId: $("#ParentComponentId").val() }, function (data) {
                container.html(data);
                document.getElementById('lineupMoveUp').addEventListener('click', handleLineupMoveUp, false);
                document.getElementById('lineupMoveDown').addEventListener('click', handleLineupMoveDown, false);
                $("#LineupPosition").val($("#newPositionInsert").index());
            });
        }
    }
    function handleLineupMoveUp() {
        // Grab the <ul>, 
        // Find the "newPositionInsert"
        // move it up on the <ul>
        // update the #LineupPosition value to the new value
        var list = document.getElementById("lineupSelection");
        var newItem = document.getElementById('newPositionInsert');
        var previousItem = newItem.previousElementSibling;
        console.log("Manager dataset is : " + previousItem.dataset.ismanager);
        if (previousItem.dataset.ismanager != "True") {
            // move the newInsert item up the list
            list.insertBefore(newItem, previousItem);
            // set the form field to the new position index
            $("#LineupPosition").val($("#newPositionInsert").index());
            console.log("New item is now at position: " + $("#newPositionInsert").index());
        } else {
            console.log("Cant move above manager");
        }

    }

    function handleLineupMoveDown() {
        var list = document.getElementById("lineupSelection");
        var newItem = document.getElementById('newPositionInsert');
        var nextItem = newItem.nextElementSibling;
        // check if the item is already at the bottom of the list
        if (nextItem) {
            // move the newInsert item up the list
            list.insertBefore(nextItem, newItem);
            // set the form field to the new position index
            $("#LineupPosition").val($("#newPositionInsert").index());
            console.log("New item is now at position: " + $("#newPositionInsert").index());
        } else {
            console.log("Item is already at the bottom of the list at position: " + $("#newPositionInsert").index());
        }
    }
    function handleComponentLineupMoveUp() {
        // Grab the <ul>,
        // Find the "newPositionInsert"
        // move it up on the <ul>
        // update the #LineupPosition value to the new value
        var list = document.getElementById("lineupSelection");
        var newItem = document.getElementById('newComponentInsert');
        var previousItem = newItem.previousElementSibling;
        if (previousItem) {
            list.insertBefore(newItem, previousItem);
            $("#LineupPosition").val($("#newComponentInsert").index());
            console.log("New item is now at position: " + $("#newComponentInsert").index());
        } else {
            console.log("Item is already at the top of the list at position: " + $("#newComponentInsert").index());
        }
    }

    function handleComponentLineupMoveDown() {
        var list = document.getElementById("lineupSelection");
        var newItem = document.getElementById('newComponentInsert');
        var nextItem = newItem.nextElementSibling;
        // check if the item is already at the bottom of the list
        if (nextItem) {
            // move the newInsert item up the list
            list.insertBefore(nextItem, newItem);
            // set the form field to the new position index
            $("#LineupPosition").val($("#newComponentInsert").index());
            console.log("New item is now at position: " + $("#newComponentInsert").index());
        } else {
            console.log("Item is already at the bottom of the list at position: " + $("#newComponentInsert").index());
        }
    }



    function HandleAssignMemberClick(e) {
        // retrieve the PositionId of the Position to which we want to assign a member
        var position = e.target.dataset.positionid;
        // retrieve the current Selected Component
        var selectedComponent = $("#Component").children("option:selected").val();
        // clear any existing modals from the container
        var container = $("#modalTarget").empty();
        // retrieve the AssignMemberModal from the server
        $.get('Roster/GetAssignMemberModalViewComponent', { positionId: position, selectedComponentId: selectedComponent }, function (data) {
            // inject the viewcomponent HTML into the container
            container.html(data);
            // register the modal's submit button handler
            var submitButton = document.getElementById('assignMemberModalSubmit');
            submitButton.addEventListener('click', handleAssignMemberModalSubmit, false);
            // show the modal
            $("#assignMemberModal").modal("show");
        })
    }

    function handleAssignMemberModalSubmit() {
        var member = $("#assignMemberModal").find("[name=MemberId]").val();
        var position = $("#assignMemberModal").find("[name=PositionId]").val();
        if ($("MemberId").children("option:selected").val() == "") {
            $("#warning").text("Please select a Member to assign.");
            return;
        }
        $("#assignMemberModal").modal("hide");
        reassignMember(member, position);
    }

    function handleReassignEmployeeModalSubmit() {
        var form = $("#reassignEmployeeModalForm");
        if ($("#PositionId").children("option:selected").val() == "") {
            $("#warning").text("Please select a Position.")
            return;
        }
        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: form.serialize(),
            success: function () {
                $("#reassignEmployeeModal").modal("hide");
                $("#getComponentButton").trigger('click');
            }
        });
    }

    function handleAddPositionToComponentModalSubmit() {
        var form = $("#addPositionToComponentModalForm");

        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: form.serialize(),
            success: function (response) {
                // the server will return a {Status = "Success"} object if validation passes.
                // this is passed from a method with an IActionResult return type, which means
                // it needs to be parsed before the properties are accessible

                if (response.success == true) {
                    $("#addPositionToComponentModal").modal("hide");
                    $("#getComponentButton").trigger('click');
                }
                else {
                    // hide the original modal
                    $("#addPositionToComponentModal").modal("hide");
                    // manually remove the modal backdrop... i don't know why it isn't removed via .modal("hide")
                    var backdrop = document.getElementsByClassName("modal-backdrop fade in");
                    [].forEach.call(backdrop, function (elem) {
                        elem.parentNode.removeChild(elem);
                    })
                    $("#modalTarget").html(response);
                    var submitButton = document.getElementById("addPositionToComponentModalSubmit");
                    submitButton.addEventListener('click', handleAddPositionToComponentModalSubmit, false);
                    $("#addPositionToComponentModal").modal("show");
                }
            },
        });
    }

    function handleEditEmployeeSubmitButtonClick() {
        var form = $("#editMemberModalForm");
        if ($("#MemberRank").children("option:selected").val() == "") {
            $("#warning").text("Please select a Rank")
            return;
        }
        else if ($("#MemberGender").children("option:selected").val() == "") {
            $("#warning").text("Please select a Gender")
            return;
        }
        else if ($("#DutyStatusId").children("option:selected").val() == "") {
            $("#warning").text("Please select a Duty Status")
            return;
        }
        else if ($("#MemberRace").children("option:selected").val() == "") {
            $("#warning").text("Please select a Race")
            return;
        }
        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: form.serialize(),
            success: function (response) {
                // the server will return a {success = true} object if validation passes.
                // this is passed from a method with an IActionResult return type, which means
                // it needs to be parsed before the properties are accessible

                if (response.success == true) {
                    // TODO: return new Member info
                    $("#editMemberModal").modal("hide");
                    $("#getComponentButton").trigger('click');
                }
                    // if the return does not have {Status = "Success"}, we assume validation failed and the response
                    // payload is a new ViewComponent with Model Validation messages
                else {
                    // hide the original modal
                    $("#editMemberModal").modal("hide");
                    // manually remove the modal backdrop... i don't know why it isn't removed via .modal("hide")
                    var backdrop = document.getElementsByClassName("modal-backdrop fade in");
                    [].forEach.call(backdrop, function (elem) {
                        elem.parentNode.removeChild(elem);
                    })
                    // re-inject the new Modal content
                    $("#modalTarget").html(response);
                    // register the new Modal submit button handler
                    var submitButton = document.getElementById("editMemberModalSubmit");
                    submitButton.addEventListener('click', handleAddPositionToComponentModalSubmit, false);
                    // show the Modal
                    $("#editMemberModal").modal("show");
                }
            },
        });
    }
    function handleDeletePosition() {
        var form = $("#deletePositionSubModalForm");
        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            dataType: 'html',
            data: form.serialize(),
            success: function () {
                $("#deletePositionSubModal").modal("hide");
                $("#addPositionToComponentModal").modal("hide");
                $("#getComponentButton").trigger('click');
            }
        })
    }

    function handleChangeDutyStatusSubmitButtonClick() {
        var form = $("#changeEmployeeStatusModalForm");
        if ($("#DutyStatus").children("option:selected").val() == "") {
            $("#warning").text("Please select a Duty Status.")
            return;
        }
        else if ($("#DutyStatus").children("option:selected").val() == $("#Member_DutyStatus_DutyStatusId").val()) {
            $("warning").text("The employee is already in the selected status");
            return;
        }
        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: form.serialize(),
            success: function (response) {
                if (response.success == true) {
                    // TODO: return new Member info
                    $("#changeEmployeeStatusModal").modal("hide");
                    $("#getComponentButton").trigger('click');
                }
                else {
                    // hide the original modal
                    $("#changeEmployeeStatusModal").modal("hide");
                    // manually remove the modal backdrop... i don't know why it isn't removed via .modal("hide")
                    var backdrop = document.getElementsByClassName("modal-backdrop fade in");
                    [].forEach.call(backdrop, function (elem) {
                        elem.parentNode.removeChild(elem);
                    })
                    // re-inject the new Modal content
                    $("#modalTarget").html(response);
                    // register the new Modal submit button handler
                    var submitButton = document.getElementById("editMemberModalSubmit");
                    submitButton.addEventListener('click', handleChangeDutyStatusSubmitButtonClick, false);
                    // show the Modal
                    $("#changeEmployeeStatusModal").modal("show");
                }
            }
        })
    }

    function handleAddEditComponentSubmit() {
        var form = $("#addEditComponentModalForm");
        // TODO: add Component Validation here
        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: form.serialize(),
            success: function (response) {
                if (response.success == true) {
                    $("#addEditComponentModal").modal("hide");
                    $("#getComponentButton").trigger('click');
                }
                else {
                    $("#addEditComponentModal").modal("hide");
                    var backdrop = document.getElementsByClassName("modal-backdrop fade in");
                    [].forEach.call(backdrop, function (elem) {
                        elem.parentNode.removeChild(elem);
                    })
                    $("#modalTarget").html(response);
                    document.getElementById('addEditComponentModalSubmit').addEventListener('click', handleAddEditComponentSubmit, false);
                    $.get("/Components/GetComponentLineupViewComponent", { parentComponentId: $("#ParentComponentId").val(), componentBeingEditedId: $("#ComponentId").val() }, function (data) {
                        $("#lineupSelectionTarget").html(data);
                        document.getElementById('lineupMoveUp').addEventListener('click', handleLineupMoveUp, false);
                        document.getElementById('lineupMoveDown').addEventListener('click', handleLineupMoveDown, false);
                        document.getElementById('ParentComponentId').addEventListener('change', handleParentComponentChange, false);
                    });
                    // show the modal
                    $("#addEditComponentModal").modal("show");
                }
            }
        })
    }

    function handleComponentShowHide(e) {
        // find the component detail section to hide
        var componentId = e.target.dataset.componentid;
        var state = e.target.dataset.state;
        switch (state) {
            case 'expanded':
                $("#componentDetailSection" + componentId).hide();
                e.target.dataset.state = 'collapsed';
                e.target.childNodes[1].classList.remove('glyphicon-chevron-up');
                e.target.childNodes[1].classList.add('glyphicon-chevron-down');
                e.target.childNodes[0].textContent = "Expand"
                break;
            case 'collapsed':
                e.target.dataset.state = 'expanded';
                e.target.childNodes[1].classList.remove('glyphicon-chevron-down');
                e.target.childNodes[1].classList.add('glyphicon-chevron-up');
                e.target.childNodes[0].textContent = "Collapse"
                $("#componentDetailSection" + componentId).show();
                break;
        }
    }

    function handleEditComponentClick(e) {
        var componentId = e.target.dataset.componentid;
        var container = $("#modalTarget").empty();
        $.get('Roster/GetAddEditComponentViewComponent', { componentId: componentId }, function (data) {
            // inject the ViewComponent HTML into the modal container
            container.html(data);
            // register the Modal's event handlers
            document.getElementById('addEditComponentModalSubmit').addEventListener('click', handleAddEditComponentSubmit, false);
            // retrieve the "Lineup" ViewComponent
            $.get("/Components/GetComponentLineupViewComponent", { parentComponentId: $("#ParentComponentId").val(), componentBeingEditedId: componentId }, function (data) {
                $("#lineupSelectionTarget").html(data);
                document.getElementById('lineupMoveUp').addEventListener('click', handleComponentLineupMoveUp, false);
                document.getElementById('lineupMoveDown').addEventListener('click', handleComponentLineupMoveDown, false);
                document.getElementById('ParentComponentId').addEventListener('change', handleParentComponentChange, false);
            });
            // show the modal
            $("#addEditComponentModal").modal("show");
        })
    }
    function handleParentComponentChange(e) {
        // retrieve the Lineup Interface view Component
        var container = $("#lineupSelectionTarget").empty();
        $.get("/Components/GetComponentLineupViewComponent", { parentComponentId: this.value, componentBeingEditedId: $("#ComponentId").val() }, function (data) {
            container.html(data);
            document.getElementById('lineupMoveUp').addEventListener('click', handleComponentLineupMoveUp, false);
            document.getElementById('lineupMoveDown').addEventListener('click', handleComponentLineupMoveDown, false);
            $("#LineupPosition").val($("#newComponentInsert").index());
        });
    };

    $("#getComponentButton").click(function () {
        var selectedComponentId = $("#Component").children("option:selected").val();
        //$.getJSON("/Roster/GetComponents", { componentId: selectedComponentId }, function (source) {});
        var container = $("#rosterContainer");
        $.get("/Roster/GetRosterViewComponent", { componentId: selectedComponentId }, function (data) {
            container.html(data);
            console.log("Grabbing new elements");
            var draggables = document.querySelectorAll('.members .member');
            console.log("Attempting to register click events: " + draggables);
            [].forEach.call(draggables, function (elem) {
                console.log("Adding handlers to: " + elem);
                elem.addEventListener('dragstart', handleDragStart, false);
                elem.addEventListener('dragenter', handleDragEnter, false)
                elem.addEventListener('dragover', handleDragOver, false);
                elem.addEventListener('dragleave', handleDragLeave, false);
                elem.addEventListener('drop', handleDrop, false);
                elem.addEventListener('dragend', handleDragEnd, false);
            });
            var moreOptionButtons = document.querySelectorAll('.moreOptions');
            [].forEach.call(moreOptionButtons, function (elem) {
                elem.addEventListener('click', showMoreOptions, false);
            });
            var moreOptionDivs = document.querySelectorAll('.drag-options');
            [].forEach.call(moreOptionDivs, function (elem) {
                elem.addEventListener('mouseleave', handleMouseOut, false);
            })
            var droppables = document.querySelectorAll('.members .insertable');
            [].forEach.call(droppables, function (elem) {
                elem.addEventListener('dragenter', handleDragEnter, false)
                elem.addEventListener('dragover', handleDragOver, false);
                elem.addEventListener('dragleave', handleDragLeave, false);
                elem.addEventListener('drop', handleDrop, false);
                elem.addEventListener('dragend', handleDragEnd, false);
            })
            var componentExpandHeaders = document.querySelectorAll('.componentHeaderMenuItem');
            [].forEach.call(componentExpandHeaders, function (elem) {
                elem.addEventListener('dragenter', handleDragEnter, false)
                elem.addEventListener('dragover', handleDragOver, false);
                elem.addEventListener('dragleave', handleDragLeave, false);
                elem.addEventListener('dragend', handleDragEnd, false);
            })
            var moreOptionsReassign = document.querySelectorAll('.drag-options-reassign');
            [].forEach.call(moreOptionsReassign, function (elem) {
                elem.addEventListener('click', handleMoreOptionsReassignEmployeeClick, false);
            })
            var moreOptionsChangeStatus = document.querySelectorAll('.drag-options-changestatus');
            [].forEach.call(moreOptionsChangeStatus, function (elem) {
                elem.addEventListener('click', handleMoreOptionsChangeDutyStatusClick, false);
            })
            var addNewPositionButtons = document.querySelectorAll('#addNewPositionButton');
            [].forEach.call(addNewPositionButtons, function (elem) {
                elem.addEventListener('click', handleAddNewPositionButtonClick, false);
            })
            var moreOptionsEdit = document.querySelectorAll('.drag-options-edit');
            [].forEach.call(moreOptionsEdit, function (elem) {
                elem.addEventListener('click', handleMoreOptionsEditMemberClick, false);
            })
            var editPositionButtons = document.querySelectorAll('.editPositionButton');
            [].forEach.call(editPositionButtons, function (elem) {
                elem.addEventListener('click', handleEditPositionButtonClick, false);
            })
            var insertableInnerAssignMemberSpans = document.querySelectorAll('.insertableInnerSelectMember');
            [].forEach.call(insertableInnerAssignMemberSpans, function (elem) {
                elem.addEventListener('click', HandleAssignMemberClick, false);
            })
            var expandCollapseComponentButtons = document.querySelectorAll('.expandCollapseButton');
            [].forEach.call(expandCollapseComponentButtons, function (elem) {
                elem.addEventListener('click', handleComponentShowHide, false);
            })
            var editComponentButtons = document.querySelectorAll('.editComponentButton');
            [].forEach.call(editComponentButtons, function (elem) {
                elem.addEventListener('click', handleEditComponentClick, false);
            })
        });
    });
