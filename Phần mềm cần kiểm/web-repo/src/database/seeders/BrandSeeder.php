<?php

namespace Database\Seeders;

use Illuminate\Database\Console\Seeds\WithoutModelEvents;
use Illuminate\Database\Seeder;
use App\Models\Brand;
use DB;
class BrandSeeder extends Seeder
{
    /**
    * Run the database seeds.
    */
    public function run(): void
    {
        // Thêm dữ liệu vào bảng brands
        DB::table('brands')->insert([
            [
                'name' => 'Apple',
                'description' => 'Apple Inc. designs, manufactures and markets smartphones, personal computers, tablets, wearables and accessories, and sells a variety of related services.',
                'created_at' => now(),
                'updated_at' => now(),
            ],
            [
                'name' => 'Samsung',
                'description' => 'Samsung, South Korean company that is one of the world\'s largest producers of electronic devices.',
                'created_at' => now(),
                'updated_at' => now(),
            ],
            [
                'name' => 'JBL',
                'description' => 'Always leading speaker trends worldwide, JBL Speakers is a famous speaker brand from the United States, making a strong impression worldwide with its unique design, distinctive sound, and combination of many advanced utilities.',
                'created_at' => now(),
                'updated_at' => now(),
            ],
            [
                'name' => 'LG',
                'description' => 'LG Electronics Inc. is a Korean multinational electronics company.',
                'created_at' => now(),
                'updated_at' => now(),
            ],
        ]);
    }
}