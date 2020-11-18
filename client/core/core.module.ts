import {
  ModuleWithProviders,
  NgModule
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './material.module';
import { ServerConfig } from './config';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HttpClientModule,
    MaterialModule
  ],
  exports: []
})
export class CoreModule {
  static forRoot(config: ServerConfig): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [
        { provide: ServerConfig, useValue: config }
      ]
    }
  }
}
