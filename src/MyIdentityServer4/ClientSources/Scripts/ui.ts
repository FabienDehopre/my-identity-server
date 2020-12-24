function domReady(): Promise<void> {
  return new Promise<void>(resolve => {
    if (document.readyState === 'loading') {
      document.addEventListener('DOMContentLoaded', () => resolve());
    } else {
      resolve();
    }
  });
}

function nextFrame(): Promise<void> {
  return new Promise<void>(resolve => {
    requestAnimationFrame(() => {
      requestAnimationFrame(() => resolve());
    });
  });
}

function afterTransition(element: Element): Promise<void> {
  return new Promise<void>(resolve => {
    const duration = parseInt(getComputedStyle(element).transitionDuration.replace('s', ''), 10) * 1000;
    setTimeout(() => resolve(), duration);
  });
}

async function enter(element: Element, transition: string): Promise<void> {
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

async function leave(element: Element, transition: string): Promise<void> {
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

function $(selector: string, scope?: Element | null): Element | null {
  return (scope ?? document).querySelector(selector);
}

function $$(selector: string, scope?: Element | null): Element[] {
  return Array.from((scope ?? document).querySelectorAll(selector));
}

function listen(type: string, selector: string, callback: (event: Event, target: Element) => void): void {
  document.addEventListener(type, event => {
    const target = (event.target as unknown as Element).closest(selector);
    if (target != null) {
      callback(event, target);
    }
  });
}

function listenOutside(type: string, selector: string, callback: (event: Event) => void): void {
  const element = $(selector) as HTMLElement | null;
  document.addEventListener(type, event => {
    if (element?.contains(event.target as unknown as Element)) {
      return;
    }

    if (element != null && element.offsetWidth < 1 && element.offsetHeight < 1) {
      return;
    }

    callback(event);
  });
}

async function toggleUserMenu(event: Event, trigger: Element): Promise<void> {
  event.stopPropagation();
  const userMenu = $('#user-menu-dropdown');
  if (userMenu == null) {
    return;
  }

  if (userMenu.classList.contains('hidden')) {
    trigger.setAttribute('aria-expanded', 'true');
    await enter(userMenu, 'fade');
  } else {
    trigger.setAttribute('aria-expanded', 'false');
    await leave(userMenu, 'fade');
  }
}

async function closeUserMenuIfOutside(): Promise<void> {
  const userMenu = $('#user-menu-dropdown');
  if (userMenu == null) {
    return;
  }

  if (!userMenu.classList.contains('hidden')) {
    const trigger = $('#user-menu-toggle-btn');
    trigger?.setAttribute('aria-expanded', 'false');
    await leave(userMenu, 'fade');
  }
}

async function toggleMobileUserMenu(event: Event, trigger: Element): Promise<void> {
  event.stopPropagation();
  const mobileUserMenu = $('#mobile-user-menu');
  if (mobileUserMenu == null) {
    return;
  }

  const menuIcon = $('svg:first-of-type', trigger);
  const closeIcon = $('svg:last-of-type', trigger);
  if (mobileUserMenu.classList.contains('hidden')) {
    mobileUserMenu.classList.remove('hidden');
    mobileUserMenu.classList.add('block');
    menuIcon?.classList.remove('block');
    menuIcon?.classList.add('hidden');
    closeIcon?.classList.remove('hidden');
    closeIcon?.classList.add('block');
  } else {
    mobileUserMenu.classList.remove('block');
    mobileUserMenu.classList.add('hidden');
    menuIcon?.classList.remove('hidden');
    menuIcon?.classList.add('block');
    closeIcon?.classList.remove('block');
    closeIcon?.classList.add('hidden');
  }
}

domReady().then(() => {
  listen('click', '#user-menu-toggle-btn', toggleUserMenu);
  listenOutside('click', '#user-menu-container', closeUserMenuIfOutside);
  listen('click', '#mobile-menu-toggle-btn', toggleMobileUserMenu);
});
