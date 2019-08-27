import { CROPSTemplatePage } from './app.po';

describe('CROPS App', function() {
  let page: CROPSTemplatePage;

  beforeEach(() => {
    page = new CROPSTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
