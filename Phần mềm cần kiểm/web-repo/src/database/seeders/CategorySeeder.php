<?php

namespace Database\Seeders;

use Illuminate\Database\Console\Seeds\WithoutModelEvents;
use Illuminate\Database\Seeder;
use App\Models\Category;
use DB;
class CategorySeeder extends Seeder
{
    /**
    * Run the database seeds.
    */
    public function run(): void
    {
        // Thêm dữ liệu vào bảng brands
        DB::table('categories')->insert([
            [
                'name' => 'Watch',
                'description' => 'Watch, portable timepiece that has a movement driven either by spring or by electricity and that is designed to be worn or carried in the pocket.',
                'created_at' => now(),
                'updated_at' => now(),
            ],
            [
                'name' => 'Phone',
                'description' => 'A mobile phone (or cellphone[a]) is a portable telephone that can make and receive calls over a radio frequency link while the user is moving within a telephone service area, as opposed to a fixed-location phone (landline phone).',
                'created_at' => now(),
                'updated_at' => now(),
            ],
            [
                'name' => 'Laptop',
                'description' => 'A laptop, sometimes called a notebook computer by manufacturers, is a battery- or AC-powered personal computer (PC) smaller than a briefcase.',
                'created_at' => now(),
                'updated_at' => now(),
            ],
            [
                'name' => 'Headphone',
                'description' => 'An equipments to put on your ear to listen music.',
                'created_at' => now(),
                'updated_at' => now(),
            ],
        ]);
    }
}