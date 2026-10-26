<?php

namespace Database\Seeders;

use Illuminate\Database\Console\Seeds\WithoutModelEvents;
use Illuminate\Database\Seeder;
use DB;
class RankSeeder extends Seeder
{
    /**
    * Run the database seeds.
    */
    public function run(): void
    {
        //
        DB::table('ranks')->insert([
        'name' => 'Đồng',
        'value' => 10000,
        'discount' => 5,
        'description' => "Cho người mới vào",
        ]);
    }
}