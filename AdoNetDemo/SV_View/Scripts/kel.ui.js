angular.module('kel.ui', []).directive('kelAjaxCombobox', ['$document', '$timeout', '$q', function($document, $timeout, $q) {

    return {
        restrict: 'E',
        templateUrl: 'Content/kel-ajax-combobox.html',
        scope: {
            ngModel : '=',
            userInput : '=',
            resultGetter : '=',
            resultList : '=',
            resultTotalRows : '=',
            selectedText : '=',
            selectedValue : '=',
            width : '=',
            pageSize : '=',

            ngChange : '&'
        },
        link: function(scope, element, attr){

            var vm = scope; // view model

            var now = new Date();
            var ticks = now.getTime();
            vm.uxUnique = 'um' + ticks; // unique marker

            if (!(vm.pageSize > 0)) {
                vm.pageSize = 10;
            }

            var _pageSize = vm.pageSize;

            vm.pageNumber = 1; // one-based
            vm.fetchDone = false;
            vm.isFromDropdown = null;
            vm.highlightedRow = null; // zero-based
            vm.oldUserInput = vm.userInput;
            vm.totalPage = 0;


            var height = _pageSize * 20 + 36;
            vm.cbStyle = { height:  height + "px" };
            var navOffset = _pageSize * 20 + 1;
            vm.cbNavStyle =  { top: navOffset + "px" };

            var _divs = angular.element(element).find('div');
            var _ccb = _divs[3]; // get the custom-combobox div

            // console.log( angular.element(_ccb) );




            angular.element(_ccb).bind('mousewheel DOMMouseScroll', function(e) {


                var scrollTo = null;

                if (e.type == 'mousewheel') {
                    scrollTo = (e.wheelDelta * -1);
                }
                else if (e.type == 'DOMMouseScroll') {
                    scrollTo = 40 * e.detail;
                }

                if (scrollTo) {
                    stopPropagation(e);

                    // angular.element(this).scrollTop(scrollTo + angular.element(this).scrollTop());


                    if (scrollTo < 0 && vm.pageNumber > 1)
                        --vm.pageNumber;

                    else if (scrollTo > 0)
                        ++vm.pageNumber;

                    if (!vm.fetchDone)
                        return;

                    doSearch();

                }

            });//scroll

            vm.goToPreviousPage = goToPreviousPage;


            function goToPreviousPage() {

                if (vm.pageNumber > 1) {
                    --vm.pageNumber;
                    doSearch();
                }
                else {
                    if (vm.highlightedRow > 0)
                    vm.highlightedRow = 0;
                }
            }

            vm.goToNextPage = goToNextPage;

            function goToNextPage() {


                if (vm.pageNumber < vm.totalPage) {
                    ++vm.pageNumber;
                    doSearch();
                }
                else {
                    if (vm.highlightedRow < vm.resultList.length - 1 ) {
                        vm.highlightedRow = vm.resultList.length - 1;
                    }
                }
            }


            function getSegmentSize() {
                return Math.ceil(vm.resultTotalRows / 100);
            }

            vm.goToNextSegment = function() {


                var ss = getSegmentSize();

                if (vm.pageNumber + ss < vm.totalPage) {
                    vm.pageNumber += ss;
                    doSearch();
                }
                else
                    goToNextPage();
            };


            vm.goToPreviousSegment = function() {


                var ss = getSegmentSize();

                if (vm.pageNumber - ss > 0 ) {
                    vm.pageNumber -= ss;
                    doSearch();
                }
                else
                    goToPreviousPage();
            };


            function doSearch() {
                vm.fetchDone = false;

                var filter = null;
                if (!vm.isFromDropdown)
                    filter = vm.userInput;
                else
                    filter = null;


                var promise = vm.resultGetter({ userInput: filter, pageNumber : vm.pageNumber, pageSize : _pageSize });

                if (promise.$promise != undefined)
                    promise = promise.$promise;

                promise.then(function(a) {
                        vm.$applyAsync(function() {

                            vm.fetchDone = true;
                            vm.totalPage = Math.ceil(vm.resultTotalRows / _pageSize);

                            if (vm.highlightedRow > vm.resultList.length - 1)
                                vm.highlightedRow = vm.resultList.length - 1;

                        });
                     });



            } // doSearch

            vm.setHighlightedRow = function(index) {
                vm.highlightedRow = index;
            };

            function selectHighlighted() {
                if (vm.highlightedRow == null) {
                    vm.userInput = vm.oldUserInput;
                }
                else if (vm.highlightedRow >= 0 && vm.highlightedRow <= vm.resultList.length - 1) {
                    var selected = vm.resultList[vm.highlightedRow];
                    vm.selectRow(selected);
                }
                else {
                    vm.userInput = vm.oldUserInput;
                }
            }

            vm.navigateByKey = function(event) {

                // 38 up 40 down

                // F4 or alt+down
                if (event.which == 115 || (event.altKey && event.which == 40)) {
                    vm.dropdown();
                }
                // down
                if (event.which == 40) {
                    if (vm.highlightedRow == null) {
                        vm.highlightedRow = 0;
                        return;
                    }

                    if (vm.highlightedRow + 1 == vm.resultList.length) {
                        if (vm.pageNumber < vm.totalPage) {
                            vm.highlightedRow = 0;
                            goToNextPage();
                        }
                        else {
                            // do nothing
                        }
                    }
                    else
                        ++vm.highlightedRow;


                    event.preventDefault();
                }
                // up
                else if (event.which == 38) {

                    if (vm.highlightedRow == null) {
                        vm.highlightedRow = _pageSize - 1;
                        return;
                    }



                    if (vm.highlightedRow - 1 == -1) {
                        if (vm.pageNumber > 1) {
                            goToPreviousPage();
                        }
                        else {
                            // do nothing
                        }
                    }
                    else
                        --vm.highlightedRow;


                    event.preventDefault();
                }
                // page up
                else if (event.which == 33) {

                    if (vm.isShowList)
                        stopPropagation(event);

                    if (!vm.fetchDone)
                        return;

                    goToPreviousPage();
                }
                // page down
                else if (event.which == 34 && vm.fetchDone) {

                    if (vm.isShowList)
                        stopPropagation(event);

                    if (!vm.fetchDone)
                        return;

                    goToNextPage();
                }
                // enter
                else if (event.which == 13 && vm.isShowList) {
                    selectHighlighted();
                    stopPropagation(event);
                }
                // escape
                else if (event.which == 27) {
                    stopPropagation(event);
                    vm.userInput = vm.oldUserInput;
                    vm.isShowList = false;
                }
                // tab
                else if (event.which == 9) {
                    if (vm.userInput == '') {
                        vm.ngModel = null;
                        vm.userInput = '';
                        vm.oldUserInput = '';
                    }
                    else {
                        if (vm.isShowList)
                            selectHighlighted();
                    }


                    vm.isShowList = false;

                }


            }; // navigateByKey

            function stopPropagation(e) {
                // this works, perhaps I'm using evergreen browsers
                e.preventDefault();
                return;
                //
                //// in case the above doesn't work, use the following
                //// http://stackoverflow.com/questions/7883807/prevent-default-event-action-not-working
                //e = e || event;/* get IE event ( not passed ) */
                //e.stopPropagation? e.stopPropagation() : e.cancelBubble = true;
            }

            function focusInput() {
                $timeout(function() {
                    var input = _divs.find('input')[0];
                    input.focus();
                },0);
            }




            vm.closePopup = function() {
                vm.isShowList = false;
            };

            vm.search = function() {
                vm.isShowList = true;
                vm.pageNumber = 1;
                vm.isFromDropdown = false;
                doSearch();
            };

            vm.selectRow = function(selected) {

                vm.ngModel = selected[vm.selectedValue];
                vm.userInput = selected[vm.selectedText];
                vm.oldUserInput = vm.userInput; // so when we press escape, we can restore to this value

                vm.isShowList = false;

                vm.highlightedRow = 0;

                vm.ngChange();
            };



            vm.dropdown = function() {
                vm.isShowList = !vm.isShowList;

                if (vm.isShowList) {
                    vm.pageNumber = 1;
                    vm.isFromDropdown = true;
                    doSearch();
                }
            };



            $document.bind('click', function(event){


                isChildOfComboBox = false;

                var target = getTarget(event);
                var parent = angular.element(target);

                for(var i = 0; i < 20; ++i) {


                    if (parent.hasClass(vm.uxUnique)) {
                        isChildOfComboBox = true;
                        break;
                    }

                    parent = parent.parent();
                }


                if (isChildOfComboBox) {
                    focusInput();
                    return;
                }

                vm.closePopup();
                vm.$apply();

            }); // bind click

            function getTarget(e) {
                var targ;
                if (!e) e = window.event;
                if (e.target) targ = e.target;
                else if (e.srcElement) targ = e.srcElement;
                if (targ.nodeType == 3) // defeat Safari bug
                    targ = targ.parentNode;
                return targ;
            }

        }//link
    };
}]);
