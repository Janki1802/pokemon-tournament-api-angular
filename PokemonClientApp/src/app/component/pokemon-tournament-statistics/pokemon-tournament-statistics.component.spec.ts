import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PokemonTournamentStatisticsComponent } from './pokemon-tournament-statistics.component';

describe('PokemonTournamentStatisticsComponent', () => {
  let component: PokemonTournamentStatisticsComponent;
  let fixture: ComponentFixture<PokemonTournamentStatisticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PokemonTournamentStatisticsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PokemonTournamentStatisticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
