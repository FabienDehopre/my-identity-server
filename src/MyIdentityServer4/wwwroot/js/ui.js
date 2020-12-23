function nextFrame() {
  return new Promise(resolve => {
    requestAnimationFrame(() => {
      requestAnimationFrame(resolve);
    });
  });
}

function afterTransition(element) {
  return new Promise(resolve => {
    const duration = Number(
      getComputedStyle(element)
        .transitionDuration
        .replace('s', '')
    ) * 1000;
    setTimeout(resolve, duration);
  });
}

async function enter(element, transition) {
  element.classList.remove('hidden');
  element.classList.add(`${transition}-enter`);
  element.classList.add(`${transition}-enter-start`);

  await nextFrame();

  element.classList.remove(`${transition}-enter-start`);
  element.classList.add(`${transition}-enter-end`);

  await afterTransition(element);

  element.classList.remove(`${transition}-enter-end`);
  element.classList.remove(`${transition}-enter`);
}

async function leave(element, transition) {
  element.classList.add(`${transition}-leave`);
  element.classList.add(`${transition}-leave-start`);

  await nextFrame();

  element.classList.remove(`${transition}-leave-start`);
  element.classList.add(`${transition}-leave-end`);

  await afterTransition(element);

  element.classList.remove(`${transition}-leave-end`);
  element.classList.remove(`${transition}-leave`);
  element.classList.add('hidden');
}

function $(selector, scope = document) {
  return scope.querySelector(selector);
}

function $$(selector, scope = document) {
  return Array.from(scope.querySelectorAll(selector));
}

function listen(type, selector, callback) {
  document.addEventListener(type, event => {
    const target = event.target.closest(selector);
    if (target) {
      callback(event, target);
    }
  });
}

function listenOutside(type, selector, callback) {
  const element = $(selector);
  document.addEventListener(type, event => {
    if (element.contains(event.target)) {
      return;
    }

    if (element.offsetWidth < 1 && element.offsetHeight < 1) {
      return;
    }

    callback(event);
  });
}

function toggleUserMenu(event, trigger) {
  event.stopPropagation();
  const userMenu = $('[role=menu][aria-labelledby=user-menu]');
  if (userMenu.classList.contains('hidden')) {
    trigger.setAttribute('aria-expanded', 'true')
    enter(userMenu, 'fade');
  } else {
    trigger.removeAttribute('aria-expanded');
    leave(userMenu, 'fade');
  }
}

function closeUserMenuIfOutside(event) {
  const userMenu = $('[role=menu][aria-labelledby=user-menu]');
  if (!userMenu.classList.contains('hidden')) {
    const trigger = $('#user-menu');
    trigger.removeAttribute('aria-expanded');
    leave(userMenu, 'fade');
  }
}

(function () {
  listen('click', '#user-menu', toggleUserMenu);
  listenOutside('click', '#user-menu-container', closeUserMenuIfOutside)
})();
