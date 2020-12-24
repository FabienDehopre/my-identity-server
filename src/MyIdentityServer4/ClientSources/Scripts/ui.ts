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

// function listen()
